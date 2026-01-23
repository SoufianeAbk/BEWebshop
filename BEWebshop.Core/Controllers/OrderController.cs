using BEWebshop.Core.Data;
using BEWebshop.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BEWebshop.Core.Controllers
{
    public class OrderController
    {
        private readonly WebshopDbContext _context;
        private readonly CartController _cartController;

        public OrderController(WebshopDbContext context)
        {
            _context = context;
            _cartController = new CartController(context);
        }

        public async Task<Order?> CreateOrderFromCartAsync(string customerName, string customerEmail, string shippingAddress)
        {
            try
            {
                var cartItems = await _cartController.GetCartItemsAsync();
                if (cartItems.Count == 0)
                    return null;

                // Validate stock availability
                var (isValid, _) = await _cartController.ValidateCartAsync();
                if (!isValid)
                    return null;

                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    CustomerName = customerName,
                    CustomerEmail = customerEmail,
                    ShippingAddress = shippingAddress,
                    Status = "Pending",
                    TotalAmount = await _cartController.GetCartTotalAsync()
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Associate cart items with order and reduce stock
                foreach (var item in cartItems)
                {
                    item.OrderId = order.Id;

                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                    }
                }

                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"Order created: ID={order.Id}, Total=€{order.TotalAmount:F2}");
                return order;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating order: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading orders: {ex.Message}");
                return new List<Order>();
            }
        }

        // ✅ NEW METHOD — Filter orders by status
        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.Status == status)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading orders by status: {ex.Message}");
                return new List<Order>();
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            try
            {
                var order = await _context.Orders.FindAsync(orderId);
                if (order == null)
                    return false;

                order.Status = newStatus;
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"Order {orderId} status updated to {newStatus}");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating order status: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    return false;

                // Restore stock
                foreach (var item in order.OrderItems)
                {
                    if (item.Product != null)
                    {
                        item.Product.Stock += item.Quantity;
                    }
                }

                order.Status = "Cancelled";
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"Order {orderId} cancelled, stock restored");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cancelling order: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    return false;

                _context.CartItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine($"Order {orderId} deleted");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting order: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAllOrdersAsync()
        {
            try
            {
                var orders = await _context.Orders.Include(o => o.OrderItems).ToListAsync();

                foreach (var order in orders)
                {
                    _context.CartItems.RemoveRange(order.OrderItems);
                }

                _context.Orders.RemoveRange(orders);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("All orders deleted");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting all orders: {ex.Message}");
                return false;
            }
        }
    }
}

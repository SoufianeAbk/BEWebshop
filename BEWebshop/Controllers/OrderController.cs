using BEWebshop.Data;
using BEWebshop.Models;
using Microsoft.EntityFrameworkCore;

namespace BEWebshop.Controllers
{
    internal class OrderController
    {
        private readonly WebshopDbContext _context;

        public OrderController(WebshopDbContext context)
        {
            _context = context;
        }

        // Create order from cart
        public async Task<Order?> CreateOrderFromCartAsync(string customerName, string customerEmail, string shippingAddress)
        {
            // Get cart items
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.OrderId == null)
                .ToListAsync();

            if (cartItems.Count == 0)
                return null;

            // Validate stock availability
            foreach (var item in cartItems)
            {
                if (item.Product == null || item.Product.Stock < item.Quantity)
                    return null; // Insufficient stock
            }

            // Calculate total
            decimal total = cartItems.Sum(ci => ci.Quantity * ci.Price);

            // Create order
            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                ShippingAddress = shippingAddress,
                TotalAmount = total,
                Status = "Pending"
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Update cart items with order ID
            foreach (var item in cartItems)
            {
                item.OrderId = order.Id;

                // Reduce product stock
                if (item.Product != null)
                {
                    item.Product.Stock -= item.Quantity;
                }
            }

            await _context.SaveChangesAsync();
            return order;
        }

        // Get all orders
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // Get order by ID
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        // Get orders by customer email
        public async Task<List<Order>> GetOrdersByCustomerEmailAsync(string email)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.CustomerEmail == email)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // Get orders by status
        public async Task<List<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // Get orders by date range
        public async Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        // Update order status
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.Status = newStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        // Cancel order
        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null || order.Status == "Delivered" || order.Status == "Cancelled")
                return false;

            // Restore product stock
            foreach (var item in order.OrderItems)
            {
                if (item.Product != null)
                {
                    item.Product.Stock += item.Quantity;
                }
            }

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        // Get order statistics
        public async Task<(int TotalOrders, decimal TotalRevenue, decimal AverageOrderValue)> GetOrderStatisticsAsync()
        {
            var orders = await _context.Orders
                .Where(o => o.Status != "Cancelled")
                .ToListAsync();

            int totalOrders = orders.Count;
            decimal totalRevenue = orders.Sum(o => o.TotalAmount);
            decimal averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            return (totalOrders, totalRevenue, averageOrderValue);
        }

        // Get recent orders
        public async Task<List<Order>> GetRecentOrdersAsync(int count = 10)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        // Get pending orders count
        public async Task<int> GetPendingOrdersCountAsync()
        {
            return await _context.Orders.CountAsync(o => o.Status == "Pending");
        }

        // Check if order exists
        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

        // Delete order (admin only - careful with this)
        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
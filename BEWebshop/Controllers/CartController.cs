using BEWebshop.Data;
using BEWebshop.Models;
using Microsoft.EntityFrameworkCore;

namespace BEWebshop.Controllers
{
    internal class CartController
    {
        private readonly WebshopDbContext _context;

        public CartController(WebshopDbContext context)
        {
            _context = context;
        }

        // Get all cart items (items without OrderId)
        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Category)
                .Where(ci => ci.OrderId == null)
                .ToListAsync();
        }

        // Get cart item by ID
        public async Task<CartItem?> GetCartItemByIdAsync(int id)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == id && ci.OrderId == null);
        }

        // Add item to cart
        public async Task<CartItem?> AddToCartAsync(int productId, int quantity)
        {
            // Check if product exists and is in stock
            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.Stock < quantity)
                return null;

            // Check if item already exists in cart
            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.OrderId == null);

            if (existingItem != null)
            {
                // Update quantity if item exists
                existingItem.Quantity += quantity;
                if (existingItem.Quantity > product.Stock)
                    existingItem.Quantity = product.Stock; // Cap at available stock

                await _context.SaveChangesAsync();
                return existingItem;
            }
            else
            {
                // Add new item to cart
                var cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price,
                    OrderId = null
                };

                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
                return cartItem;
            }
        }

        // Update cart item quantity
        public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int newQuantity)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.OrderId == null);

            if (cartItem == null || cartItem.Product == null)
                return false;

            if (newQuantity <= 0)
            {
                // Remove item if quantity is 0 or negative
                _context.CartItems.Remove(cartItem);
            }
            else if (newQuantity <= cartItem.Product.Stock)
            {
                cartItem.Quantity = newQuantity;
            }
            else
            {
                // Cap at available stock
                cartItem.Quantity = cartItem.Product.Stock;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Remove item from cart
        public async Task<bool> RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.OrderId == null);

            if (cartItem == null)
                return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        // Clear cart
        public async Task ClearCartAsync()
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.OrderId == null)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        // Get cart total
        public async Task<decimal> GetCartTotalAsync()
        {
            return await _context.CartItems
                .Where(ci => ci.OrderId == null)
                .SumAsync(ci => ci.Quantity * ci.Price);
        }

        // Get cart item count
        public async Task<int> GetCartItemCountAsync()
        {
            return await _context.CartItems
                .Where(ci => ci.OrderId == null)
                .SumAsync(ci => ci.Quantity);
        }

        // Validate cart (check if all items are still in stock)
        public async Task<(bool IsValid, List<string> Errors)> ValidateCartAsync()
        {
            var errors = new List<string>();
            var cartItems = await GetCartItemsAsync();

            foreach (var item in cartItems)
            {
                if (item.Product == null)
                {
                    errors.Add($"Product not found for cart item {item.Id}");
                    continue;
                }

                if (item.Product.Stock < item.Quantity)
                {
                    errors.Add($"Insufficient stock for {item.Product.Name}. Available: {item.Product.Stock}, Requested: {item.Quantity}");
                }
            }

            return (errors.Count == 0, errors);
        }

        // Update cart prices (in case product prices changed)
        public async Task UpdateCartPricesAsync()
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.OrderId == null)
                .ToListAsync();

            foreach (var item in cartItems)
            {
                if (item.Product != null)
                {
                    item.Price = item.Product.Price;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
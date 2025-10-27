using BEWebshop.Data;
using BEWebshop.Models;
using Microsoft.EntityFrameworkCore;

namespace BEWebshop.Controllers
{
    internal class ProductController
    {
        private readonly WebshopDbContext _context;

        public ProductController(WebshopDbContext context)
        {
            _context = context;
        }

        // Get all products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        // Get product by ID
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Get products by category
        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        // Search products by name
        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();
        }

        // Add new product
        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // Update product
        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Delete product
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update stock
        public async Task<bool> UpdateStockAsync(int productId, int newStock)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return false;

            product.Stock = newStock;
            await _context.SaveChangesAsync();
            return true;
        }

        // Check if product is in stock
        public async Task<bool> IsInStockAsync(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            return product != null && product.Stock >= quantity;
        }

        // Get low stock products
        public async Task<List<Product>> GetLowStockProductsAsync(int threshold = 10)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.Stock <= threshold)
                .ToListAsync();
        }
    }
}
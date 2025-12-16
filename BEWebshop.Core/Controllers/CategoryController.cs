using BEWebshop.Core.Data;
using BEWebshop.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BEWebshop.Controllers
{
    public class CategoryController
    {
        private readonly WebshopDbContext _context;

        public CategoryController(WebshopDbContext context)
        {
            _context = context;
        }

        // Get all categories
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }

        // Get category by ID
        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Get category by name
        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        // Add new category
        public async Task<Category> AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        // Update category
        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Delete category
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            // Check if category has products
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
                return false; // Cannot delete category with products

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get categories with product count
        public async Task<List<(Category Category, int ProductCount)>> GetCategoriesWithProductCountAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();

            return categories.Select(c => (c, c.Products.Count)).ToList();
        }

        // Check if category exists
        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        // Check if category name is unique
        public async Task<bool> IsCategoryNameUniqueAsync(string name, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return !await _context.Categories.AnyAsync(c => c.Name == name && c.Id != excludeId.Value);
            }
            return !await _context.Categories.AnyAsync(c => c.Name == name);
        }
    }
}
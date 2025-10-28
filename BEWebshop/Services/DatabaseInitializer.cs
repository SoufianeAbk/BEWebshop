using BEWebshop.Data;
using BEWebshop.Models;
using Microsoft.EntityFrameworkCore;

namespace BEWebshop.Services
{
    internal class DatabaseInitializer
    {
        public static void Initialize(WebshopDbContext context)
        {
            try
            {
                // Ensure database is created
                context.Database.EnsureCreated();

                // Check if we need to seed data
                if (!context.Categories.Any())
                {
                    SeedData(context);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error
                System.Windows.MessageBox.Show(
                    $"Database initialization failed: {ex.Message}\n\nInner Exception: {ex.InnerException?.Message}",
                    "Database Error",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                throw;
            }
        }

        private static void SeedData(WebshopDbContext context)
        {
            // Seed Categories
            var categories = new List<Category>
            {
                new Category { Name = "Electronics", Description = "Electronic devices and gadgets" },
                new Category { Name = "Books", Description = "Books and publications" },
                new Category { Name = "Clothing", Description = "Apparel and accessories" },
                new Category { Name = "Home & Garden", Description = "Home and garden products" },
                new Category { Name = "Sports", Description = "Sports and outdoor equipment" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Products
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Laptop",
                    Description = "High-performance laptop with 16GB RAM and 512GB SSD",
                    Price = 999.99m,
                    Stock = 10,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Smartphone",
                    Description = "Latest smartphone model with 5G connectivity",
                    Price = 699.99m,
                    Stock = 25,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Wireless Headphones",
                    Description = "Noise-cancelling wireless headphones",
                    Price = 199.99m,
                    Stock = 50,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "C# Programming Book",
                    Description = "Comprehensive guide to C# programming",
                    Price = 49.99m,
                    Stock = 30,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Fiction Novel",
                    Description = "Bestselling fiction novel",
                    Price = 19.99m,
                    Stock = 100,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "T-Shirt",
                    Description = "100% cotton t-shirt, available in multiple colors",
                    Price = 19.99m,
                    Stock = 100,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Jeans",
                    Description = "Classic blue jeans",
                    Price = 59.99m,
                    Stock = 75,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Garden Tools Set",
                    Description = "Complete set of essential garden tools",
                    Price = 89.99m,
                    Stock = 20,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Running Shoes",
                    Description = "Professional running shoes with cushioned sole",
                    Price = 129.99m,
                    Stock = 40,
                    CategoryId = 5
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
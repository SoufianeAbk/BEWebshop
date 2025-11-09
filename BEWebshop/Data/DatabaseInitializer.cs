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
                new Category { Name = "Games", Description = "Video games and gaming products" },
                new Category { Name = "Elektronica", Description = "Electronic devices and accessories" },
                new Category { Name = "Sport", Description = "Sports equipment and gear" },
                new Category { Name = "Kleding", Description = "Clothing and apparel" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Products
            var products = new List<Product>
            {
                // Games (CategoryId = 1)
                new Product
                {
                    Name = "PlayStation 5",
                    Description = "Next-gen gaming console with 825GB SSD",
                    Price = 499.99m,
                    Stock = 15,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Xbox Series X",
                    Description = "Powerful gaming console with 1TB storage",
                    Price = 499.99m,
                    Stock = 12,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Elden Ring",
                    Description = "Fantasy action RPG game",
                    Price = 59.99m,
                    Stock = 30,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Gaming Mouse",
                    Description = "RGB gaming mouse with 16000 DPI",
                    Price = 79.99m,
                    Stock = 50,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Gaming Keyboard",
                    Description = "Mechanical gaming keyboard with RGB lighting",
                    Price = 149.99m,
                    Stock = 25,
                    CategoryId = 1
                },

                // Elektronica (CategoryId = 2)
                new Product
                {
                    Name = "Laptop",
                    Description = "High-performance laptop with 16GB RAM and 512GB SSD",
                    Price = 999.99m,
                    Stock = 10,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Smartphone",
                    Description = "Latest smartphone model with 5G connectivity",
                    Price = 699.99m,
                    Stock = 20,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Wireless Headphones",
                    Description = "Noise-cancelling wireless headphones with 40h battery",
                    Price = 199.99m,
                    Stock = 40,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Tablet",
                    Description = "10-inch tablet with stylus support",
                    Price = 349.99m,
                    Stock = 18,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Smartwatch",
                    Description = "Fitness tracking smartwatch with heart rate monitor",
                    Price = 199.99m,
                    Stock = 35,
                    CategoryId = 2
                },

                // Sport (CategoryId = 3)
                new Product
                {
                    Name = "Running Shoes",
                    Description = "Professional running shoes with cushioned sole",
                    Price = 129.99m,
                    Stock = 45,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Yoga Mat",
                    Description = "Non-slip yoga mat 6mm thickness",
                    Price = 24.99m,
                    Stock = 60,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Dumbbell Set",
                    Description = "20kg adjustable dumbbell set",
                    Price = 89.99m,
                    Stock = 20,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Bicycle",
                    Description = "Mountain bike with 21-speed gear",
                    Price = 399.99m,
                    Stock = 8,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Sports Water Bottle",
                    Description = "1L insulated water bottle",
                    Price = 34.99m,
                    Stock = 100,
                    CategoryId = 3
                },

                // Kleding (CategoryId = 4)
                new Product
                {
                    Name = "T-Shirt",
                    Description = "100% cotton t-shirt, available in multiple colors",
                    Price = 19.99m,
                    Stock = 150,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Jeans",
                    Description = "Classic blue jeans with comfortable fit",
                    Price = 59.99m,
                    Stock = 80,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Winter Jacket",
                    Description = "Waterproof winter jacket with thermal lining",
                    Price = 149.99m,
                    Stock = 25,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Socks Pack",
                    Description = "Pack of 12 comfortable cotton socks",
                    Price = 14.99m,
                    Stock = 200,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Hoodie",
                    Description = "Comfortable cotton blend hoodie",
                    Price = 49.99m,
                    Stock = 50,
                    CategoryId = 4
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
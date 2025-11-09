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
            // Seed Categories - 4 categories in English
            var categories = new List<Category>
            {
                new Category { Name = "Electronics", Description = "Electronic devices and accessories" },
                new Category { Name = "Books", Description = "Books and educational materials" },
                new Category { Name = "Clothing", Description = "Clothing and apparel" },
                new Category { Name = "Sports", Description = "Sports equipment and gear" }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Products - 5 products per category (20 total)
            var products = new List<Product>
            {
                // Electronics (CategoryId = 1) - 5 products
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
                    Description = "Noise-cancelling wireless headphones with 40h battery",
                    Price = 199.99m,
                    Stock = 40,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Tablet",
                    Description = "10-inch tablet with stylus support",
                    Price = 349.99m,
                    Stock = 18,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Smartwatch",
                    Description = "Fitness tracking smartwatch with heart rate monitor",
                    Price = 199.99m,
                    Stock = 35,
                    CategoryId = 1
                },

                // Books (CategoryId = 2) - 5 products
                new Product
                {
                    Name = "C# Programming Book",
                    Description = "Comprehensive guide to C# programming language",
                    Price = 49.99m,
                    Stock = 50,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Web Development Guide",
                    Description = "Learn HTML, CSS, JavaScript and modern frameworks",
                    Price = 54.99m,
                    Stock = 35,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Database Design Book",
                    Description = "Master SQL and database architecture",
                    Price = 44.99m,
                    Stock = 28,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Python for Beginners",
                    Description = "Start your Python programming journey",
                    Price = 39.99m,
                    Stock = 60,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Advanced Algorithms",
                    Description = "Deep dive into algorithmic problem solving",
                    Price = 59.99m,
                    Stock = 22,
                    CategoryId = 2
                },

                // Clothing (CategoryId = 3) - 5 products
                new Product
                {
                    Name = "T-Shirt",
                    Description = "100% cotton t-shirt, available in multiple colors",
                    Price = 19.99m,
                    Stock = 150,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Jeans",
                    Description = "Classic blue jeans with comfortable fit",
                    Price = 59.99m,
                    Stock = 80,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Winter Jacket",
                    Description = "Waterproof winter jacket with thermal lining",
                    Price = 149.99m,
                    Stock = 25,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Socks Pack",
                    Description = "Pack of 12 comfortable cotton socks",
                    Price = 14.99m,
                    Stock = 200,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Hoodie",
                    Description = "Comfortable cotton blend hoodie",
                    Price = 49.99m,
                    Stock = 50,
                    CategoryId = 3
                },

                // Sports (CategoryId = 4) - 5 products
                new Product
                {
                    Name = "Running Shoes",
                    Description = "Professional running shoes with cushioned sole",
                    Price = 129.99m,
                    Stock = 45,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Yoga Mat",
                    Description = "Non-slip yoga mat 6mm thickness",
                    Price = 24.99m,
                    Stock = 60,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Dumbbell Set",
                    Description = "20kg adjustable dumbbell set",
                    Price = 89.99m,
                    Stock = 20,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Bicycle",
                    Description = "Mountain bike with 21-speed gear",
                    Price = 399.99m,
                    Stock = 8,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Sports Water Bottle",
                    Description = "1L insulated water bottle",
                    Price = 34.99m,
                    Stock = 100,
                    CategoryId = 4
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
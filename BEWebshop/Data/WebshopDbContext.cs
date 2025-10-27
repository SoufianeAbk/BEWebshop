using BEWebshop.Models;
using Microsoft.EntityFrameworkCore;

namespace BEWebshop.Data
{
    internal class WebshopDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection here
            // Example for SQL Server:
            // optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BEWebshopDb;Trusted_Connection=True;");

            // Example for SQLite (good for development):
            optionsBuilder.UseSqlite("Data Source=webshop.db");

            // Enable lazy loading if needed
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                // Configure relationship with Category
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index on CategoryId for better query performance
                entity.HasIndex(e => e.CategoryId);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CustomerEmail).IsRequired().HasMaxLength(200);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Status).HasMaxLength(50);

                // Index on OrderDate for better query performance
                entity.HasIndex(e => e.OrderDate);
            });

            // Configure CartItem entity
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

                // Configure relationship with Product
                entity.HasOne(e => e.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with Order (optional)
                entity.HasOne(e => e.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(e => e.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(false);

                // Indexes for better query performance
                entity.HasIndex(e => e.ProductId);
                entity.HasIndex(e => e.OrderId);
            });

            // Seed initial data (optional)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" },
                new Category { Id = 2, Name = "Books", Description = "Books and publications" },
                new Category { Id = 3, Name = "Clothing", Description = "Apparel and accessories" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, Stock = 10, CategoryId = 1 },
                new Product { Id = 2, Name = "Smartphone", Description = "Latest smartphone model", Price = 699.99m, Stock = 25, CategoryId = 1 },
                new Product { Id = 3, Name = "C# Programming Book", Description = "Comprehensive C# guide", Price = 49.99m, Stock = 50, CategoryId = 2 },
                new Product { Id = 4, Name = "T-Shirt", Description = "Cotton t-shirt", Price = 19.99m, Stock = 100, CategoryId = 3 }
            );
        }
    }
}
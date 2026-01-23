using System.Configuration;
using System.Data;
using System.Windows;
using BEWebshop.Core.Data;
using BEWebshop.Core.Models;
using BEWebshop.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BEWebshop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize SQLite
            SQLitePCL.Batteries.Init();

            // Configure dependency injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Show login window
            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                // Show main window after successful login
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                // User cancelled login, exit application
                Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register DbContext
            services.AddDbContext<WebshopDbContext>(options =>
                options.UseSqlite("Data Source=webshop.db")
                       .UseLazyLoadingProxies());

            // Register Identity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<WebshopDbContext>()
            .AddDefaultTokenProviders();

            // Register services
            services.AddScoped<AuthenticationService>();
        }
    }
}
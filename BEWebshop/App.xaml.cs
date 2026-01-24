using System.Configuration;
using System.Data;
using System.Windows;
using BEWebshop.Core.Data;
using BEWebshop.Core.Models;
using BEWebshop.Core.Services;
using BEWebshop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

            // Initialize the database with migrations
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<WebshopDbContext>();

                // Use Migrate() instead of EnsureCreated() to properly create Identity tables
                context.Database.Migrate();

                DatabaseInitializer.Initialize(context);
            }

            // Show login window
            var loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                // Show main window after successful login
                var mainWindow = new MainWindow();
                MainWindow = mainWindow;
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
            // Register DbContext as Scoped
            services.AddDbContext<WebshopDbContext>(options =>
                options.UseSqlite("Data Source=webshop.db")
                       .UseLazyLoadingProxies());

            // Manually register Identity components for WPF
            services.AddScoped<UserManager<User>>();
            services.AddScoped<IUserStore<User>, UserStore<User, IdentityRole, WebshopDbContext>>();
            services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, WebshopDbContext>>();

            // Password hasher
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // User validator
            services.AddScoped<IUserValidator<User>, UserValidator<User>>();
            services.AddScoped<IPasswordValidator<User>, PasswordValidator<User>>();

            // Configure Identity options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            // Register AuthenticationService as Singleton so CurrentUser persists
            services.AddSingleton<AuthenticationService>();

            // Register ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<CartViewModel>();
            services.AddTransient<OrderViewModel>();
        }
    }
}
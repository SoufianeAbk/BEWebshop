using BEWebshop.Core.Data;
using BEWebshop.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text;

namespace BEWebshop.Core.Services
{
    public class AuthenticationService
    {
        private readonly IServiceProvider _serviceProvider;
        private User? _currentUser;

        public AuthenticationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public User? CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;

        public async Task<(bool Success, string Message)> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<WebshopDbContext>();

                // Check if user already exists
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser != null)
                {
                    return (false, "Email already registered");
                }

                // Create new user
                var user = new User
                {
                    UserName = email,
                    Email = email,
                    NormalizedEmail = email.ToUpper(),
                    NormalizedUserName = email.ToUpper(),
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                // Hash password
                user.PasswordHash = HashPassword(password);

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return (true, "Registration successful");
            }
            catch (Exception ex)
            {
                return (false, $"Registration failed: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> LoginAsync(string email, string password)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<WebshopDbContext>();

                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return (false, "Invalid email or password");
                }

                // Verify password
                if (VerifyPassword(password, user.PasswordHash ?? ""))
                {
                    _currentUser = user;
                    return (true, "Login successful");
                }

                return (false, "Invalid email or password");
            }
            catch (Exception ex)
            {
                return (false, $"Login failed: {ex.Message}");
            }
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            if (_currentUser == null)
                return false;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<WebshopDbContext>();

                // Verify current password
                if (!VerifyPassword(currentPassword, _currentUser.PasswordHash ?? ""))
                    return false;

                // Update password
                _currentUser.PasswordHash = HashPassword(newPassword);

                // Update in database
                var dbUser = await context.Users.FindAsync(_currentUser.Id);
                if (dbUser != null)
                {
                    dbUser.PasswordHash = _currentUser.PasswordHash;
                    await context.SaveChangesAsync();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }
}
using BEWebshop.Core.Data;
using BEWebshop.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace BEWebshop.Core.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private User? _currentUser;

        public AuthenticationService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public User? CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;

        public async Task<(bool Success, string Message)> RegisterAsync(string email, string password, string firstName, string lastName)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return (true, "Registration successful");
            }

            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<(bool Success, string Message)> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (false, "Invalid email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _currentUser = user;
                return (true, "Login successful");
            }

            return (false, "Invalid email or password");
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
        {
            if (_currentUser == null)
                return false;

            var result = await _userManager.ChangePasswordAsync(_currentUser, currentPassword, newPassword);
            return result.Succeeded;
        }
    }
}
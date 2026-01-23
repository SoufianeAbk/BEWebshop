using System.Windows;
using BEWebshop.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BEWebshop
{
    public partial class LoginWindow : Window
    {
        private readonly AuthenticationService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = App.ServiceProvider.GetRequiredService<AuthenticationService>();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var (success, message) = await _authService.LoginAsync(email, password);

            if (success)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(message, "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            if (registerWindow.ShowDialog() == true)
            {
                MessageBox.Show("Registration successful! Please login.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
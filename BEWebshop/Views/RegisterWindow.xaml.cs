using System.Windows;
using BEWebshop.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BEWebshop
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthenticationService _authService;

        public RegisterWindow()
        {
            InitializeComponent();
            _authService = App.ServiceProvider.GetRequiredService<AuthenticationService>();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;
            var confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill in all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var (success, message) = await _authService.RegisterAsync(email, password);

            if (success)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(message, "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
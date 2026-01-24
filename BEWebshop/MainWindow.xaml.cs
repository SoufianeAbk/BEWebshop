using System.Windows;
using BEWebshop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BEWebshop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                System.Diagnostics.Debug.WriteLine("MainWindow initializing...");

                // Get MainViewModel from DI container
                var viewModel = App.ServiceProvider.GetRequiredService<MainViewModel>();
                DataContext = viewModel;

                System.Diagnostics.Debug.WriteLine("MainViewModel set as DataContext");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in MainWindow constructor: {ex.Message}");
                MessageBox.Show($"Error initializing main window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("MainWindow closing...");
            base.OnClosed(e);
        }
    }
}
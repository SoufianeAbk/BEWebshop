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
            // Get MainViewModel from DI container
            DataContext = App.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}
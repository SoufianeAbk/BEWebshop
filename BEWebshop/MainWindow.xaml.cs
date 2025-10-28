using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using BEWebshop.ViewModels;

namespace BEWebshop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }

    // Simple converter for button highlighting in navigation
    public class ViewToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value?.ToString() == parameter?.ToString())
            {
                return new SolidColorBrush(Color.FromRgb(0, 90, 158)); // Darker blue for active
            }
            return new SolidColorBrush(Color.FromRgb(0, 122, 204)); // Regular blue
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
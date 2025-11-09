using System.Windows.Data;
using System.Windows.Media;

namespace BEWebshop
{
    public class ViewToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo? culture)
        {
            if (value == null || parameter == null)
                return new SolidColorBrush(Color.FromRgb(0, 122, 204)); // #007ACC

            string currentView = value.ToString() ?? "";
            string buttonView = parameter.ToString() ?? "";

            if (currentView == buttonView)
                return new SolidColorBrush(Color.FromRgb(0, 90, 158)); // #005A9E - darker blue when selected

            return new SolidColorBrush(Color.FromRgb(0, 122, 204)); // #007ACC - normal blue
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace WMPFluent.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
            {
                // Return green color when the boolean value is true
                return new SolidColorBrush((Windows.UI.Color)App.Current.Resources["SystemAccentColor"]);
            }
            else
            {
                // Return red color when the boolean value is false
                return (SolidColorBrush)App.Current.Resources["ApplicationForegroundThemeBrush"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

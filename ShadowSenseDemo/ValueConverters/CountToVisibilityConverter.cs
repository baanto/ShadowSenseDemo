using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ShadowSenseDemo.ValueConverters
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var flag = false;
            if (parameter != null)
            {
                if (bool.Parse((string)parameter))
                {
                    flag = !flag;
                }
            }

            int? count = (int)value;
            if (count == null)
                return Visibility.Collapsed;

            if (flag)
                return count.Value > 0 ? Visibility.Visible : Visibility.Collapsed;
            else
                return count.Value > 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("CountToVisibilityConverter can only be used OneWay.");
        }
    }

}

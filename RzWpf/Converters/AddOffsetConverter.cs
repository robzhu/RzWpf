using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    /// <summary>
    /// Given a value and a parameter, this converter returns the sum of the two.
    /// </summary>
    public class AddOffsetConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                double returnValue = (double)value + double.Parse( parameter as string );
                return returnValue;
            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    public class BoolToValueConverter<T> : IValueConverter
    {
        public T FalseValue { get; set; }
        public T TrueValue { get; set; }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                if( value == null )
                    return DependencyProperty.UnsetValue;

                return (bool)value ? TrueValue : FalseValue;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                return ( value != null ) ? value.Equals( TrueValue ) : DependencyProperty.UnsetValue;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}

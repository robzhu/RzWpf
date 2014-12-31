using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    public sealed class ObjectToTypeStringConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null ) return DependencyProperty.UnsetValue;
            return value.GetType().Name;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) 
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    public sealed class ObjectTypeEqualityToVisibilityConverter : IValueConverter
    {
        public Visibility ValueIfEqual { get; set; }
        public Visibility ValueIfNotEqual { get; set; }

        public ObjectTypeEqualityToVisibilityConverter()
        {
            ValueIfEqual = Visibility.Visible;
            ValueIfNotEqual = Visibility.Collapsed;
        }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value == null ) return ValueIfNotEqual;

            string typeString = parameter as string;
            if( string.IsNullOrEmpty( typeString ) ) return ValueIfNotEqual;

            return ( value.GetType().Name == typeString ) ? ValueIfEqual : ValueIfNotEqual;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) 
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

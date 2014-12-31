using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    [ValueConversion( typeof( object ), typeof( Visibility ) )]
    public sealed class ObjectEqualityToVisibilityConverter : IValueConverter
    {
        public Visibility ValueIfEqual { get; set; }
        public Visibility ValueIfNotEqual { get; set; }

        public ObjectEqualityToVisibilityConverter()
        {
            ValueIfEqual = Visibility.Visible;
            ValueIfNotEqual = Visibility.Collapsed;
        }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return ( value == parameter ) ? ValueIfEqual : ValueIfNotEqual;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

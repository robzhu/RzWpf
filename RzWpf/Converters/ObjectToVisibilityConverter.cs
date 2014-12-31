using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    [ValueConversion( typeof( object ), typeof( Visibility ) )]
    public class ObjectToVisibilityConverter : IValueConverter
    {
        public Visibility NullValue { get; set; }
        public Visibility NonNullValue { get; set; }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return ( value == null ) ? NullValue : NonNullValue;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}

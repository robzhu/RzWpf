using System;
using System.Windows.Data;

namespace RzWpf
{
    public class ReferenceNullToValueConverter<T> : IValueConverter
    {
        public T NullValue { get; set; }
        public T NonNullValue { get; set; }

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return ( value == null ) ? NullValue : NonNullValue;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}

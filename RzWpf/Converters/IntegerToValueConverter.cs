using System;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    public class IntegerToValueConverter<T> : IValueConverter
    {
        public T NegativeValue { get; set; }
        public T ZeroValue { get; set; }
        public T PositiveValue { get; set; }

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if( value == null )
                return DependencyProperty.UnsetValue;

            int val = (int)value;

            if( val < 0 )
                return NegativeValue;
            else if( val > 0 )
                return PositiveValue;
            else 
                return ZeroValue;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}

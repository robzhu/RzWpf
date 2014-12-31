using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    [ValueConversion( typeof( float ), typeof( string ) )]
    public class FloatToPercentStringConverter : IValueConverter
    {
        public string StringFormat { get; set; }

        public FloatToPercentStringConverter()
        {
            StringFormat = "{0:0}%";
        }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                return string.Format( StringFormat, (float)value * 100 );
            }
            catch
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

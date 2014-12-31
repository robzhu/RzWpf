using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    [ValueConversion( typeof( double ), typeof( double ) )]
    public class MillisecondsToSecondsConverter : IValueConverter
    {
        public string Format
        {
            set
            {
                _format = "{" + value + "}";
            }
        }

        private string _format;

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                return string.Format( _format, ( (double)value ) / 1000 );
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

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RzWpf
{
    [ValueConversion( typeof( Color ), typeof( SolidColorBrush ) )]
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                return new SolidColorBrush( (Color)value );
            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;
            }
           
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            SolidColorBrush brush = value as SolidColorBrush;
            if( brush != null )
            {
                return brush.Color;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}

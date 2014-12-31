using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    [ValueConversion( typeof( double ), typeof( double ) )]
    public class LogarithmicScaleConverter : IValueConverter
    {
        public double ScaleBase { get; set; }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            double scaleExponent = (double)value;
            return Math.Pow( ScaleBase, scaleExponent );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

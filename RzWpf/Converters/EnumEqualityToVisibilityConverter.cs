using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    /// <summary>
    /// Given an enum and a corresponding string value (defined via parameter), this converter checks to see
    /// if the two values match a returns the visibility specified by ValueIfEqual or ValueIfNotEqual
    /// </summary>
    [ValueConversion( typeof( bool ), typeof( Enum ) )]
    public class EnumEqualityToVisibilityConverter : IValueConverter
    {
        public Visibility ValueIfEqual { get; set; }
        public Visibility ValueIfNotEqual { get; set; }

        /// <summary>
        /// Converts the equality of the value and parameter to a visibility.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <param name="targetType">Only used by data binding</param>
        /// <param name="parameter">The string value to check.  For the two to be equal, thus must be the enum.ToString() value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>ValueIfEqual if value and parameter are the string/enum equivalents.  ValueIfNotEqual if they are different.  DependencyProperty.Unset if not applicable.</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string parameterString = parameter as string;
            if( parameterString == null )
            {
                return DependencyProperty.UnsetValue;
            }

            if( ( value == null ) || ( !value.GetType().IsEnum ) )
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                object parameterValue = Enum.Parse( value.GetType(), parameterString );

                return parameterValue.Equals( value ) ? ValueIfEqual : ValueIfNotEqual;
            }
            catch( ArgumentException )
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

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    /// <summary>
    /// Given an enum and a corresponding string value (defined via parameter), this converter checks to see
    /// if the two values match.  
    /// </summary>
    /// <remarks>Use this class to bind radio button selection to specific enum values.</remarks>
    [ValueConversion( typeof( bool ), typeof( Enum ) )]
    public class EnumToBoolConverter : IValueConverter
    {
        #region IValueConverter
        /// <summary>
        /// Converts the equality of the value and parameter to a boolean result.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <param name="targetType">Only used by data binding</param>
        /// <param name="parameter">The string value to check.  For the two to be equal, thus must be the enum.ToString() value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>True if value and parameter are the string/enum equivalents.  False if they are different.  DependencyProperty.Unset if not applicable.</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string parameterString = parameter as string;
            if( parameterString == null )
            {
                return DependencyProperty.UnsetValue;
            }

            if( null == value )
            {
                throw new ArgumentNullException( "value" );
            }

            if( !value.GetType().IsEnum )
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                object parameterValue = Enum.Parse( value.GetType(), parameterString );

                return parameterValue.Equals( value );
            }
            catch( ArgumentException )
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// Converts the parameter back into the enum value it parses to.  
        /// </summary>
        /// <param name="value">Only used by data binding.</param>
        /// <param name="targetType">The enumeration type.</param>
        /// <param name="parameter">The string value to convert to the enum.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The enumerated value parsed from parameter.  DependencyProperty.Unset if parameter could not be parsed.</returns>
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string parameterString = parameter as string;
            if( string.IsNullOrEmpty( parameterString ) )
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                return Enum.Parse( targetType, parameterString );
            }
            catch( ArgumentException )
            {
                return DependencyProperty.UnsetValue;
            }
        }
        #endregion
    }
}

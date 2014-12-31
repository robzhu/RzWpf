using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    /// <summary>
    /// This class is responsible for converting between bools and WPF visibility enumerations.
    /// </summary>
    [ValueConversion( typeof( bool ), typeof( Visibility ) )]
    public class BoolToVisibilityConverter : BoolToValueConverter<Visibility> 
    {
        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }
    }
}

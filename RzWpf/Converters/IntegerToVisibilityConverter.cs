using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    /// <summary>
    /// This class is responsible for converting between ints and WPF visibility enumerations.
    /// </summary>
    [ValueConversion( typeof( int ), typeof( Visibility ) )]
    public class IntegerToVisibilityConverter : IntegerToValueConverter<Visibility> { }
}

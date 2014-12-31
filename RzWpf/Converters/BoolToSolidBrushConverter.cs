using System.Windows.Data;
using System.Windows.Media;

namespace RzWpf
{
    /// <summary>
    /// This class is responsible for converting between bools and WPF brushes.
    /// </summary>
    [ValueConversion( typeof( bool ), typeof( Brush ) )]
    public class BoolToBrushConverter : BoolToValueConverter<Brush> 
    {
        public BoolToBrushConverter()
        {
            TrueValue = new SolidColorBrush( Colors.GreenYellow );
            FalseValue = new SolidColorBrush( Colors.OrangeRed );
        }
    }
}

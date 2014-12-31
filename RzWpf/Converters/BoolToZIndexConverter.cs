using System.Windows.Data;

namespace RzWpf
{
    /// <summary>
    /// This class is responsible for converting between bools and WPF visibility enumerations.
    /// </summary>
    [ValueConversion( typeof( bool ), typeof( int ) )]
    public class BoolToZIndexConverter : BoolToValueConverter<int> { }
}

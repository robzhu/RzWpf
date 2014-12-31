using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    /// <summary>
    /// This class is responsible for converting between bools and WPF visibility enumerations.
    /// </summary>
    [ValueConversion( typeof( bool ), typeof( FlowDirection ) )]
    public class BoolToFlowDirectionConverter : BoolToValueConverter<FlowDirection> 
    {
        public BoolToFlowDirectionConverter()
        {
            TrueValue = FlowDirection.RightToLeft;
            FalseValue = FlowDirection.LeftToRight;
        }
    }
}

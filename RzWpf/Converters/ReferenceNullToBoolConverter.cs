using System.Windows.Data;

namespace RzWpf
{
    [ValueConversion( typeof( object ), typeof( bool ) )]
    public class ReferenceNullToBoolConverter : ReferenceNullToValueConverter<bool> 
    {
    }
}

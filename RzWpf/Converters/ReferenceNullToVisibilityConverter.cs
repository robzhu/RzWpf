using System.Windows;
using System.Windows.Data;

namespace RzWpf
{
    [ValueConversion( typeof( object ), typeof( Visibility ) )]
    public class ReferenceNullToVisibilityConverter : ReferenceNullToValueConverter<Visibility> 
    {
    }
}

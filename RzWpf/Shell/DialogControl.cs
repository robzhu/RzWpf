using System.Windows;
using System.Windows.Controls;

namespace RzWpf
{
    public class DialogControl : ContentControl
    {
        static DialogControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( DialogControl ), new FrameworkPropertyMetadata( typeof( DialogControl ) ) );
        }
    }
}

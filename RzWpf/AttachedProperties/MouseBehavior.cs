using System.Collections.Generic;
using System.Windows;

namespace RzWpf
{
    public static class MouseBehavior
    {
        private static readonly List<FrameworkElement> _wiredUpElements = new List<FrameworkElement>();
        private static readonly object _wiredUpElementsLock = new object();

        public static readonly DependencyProperty SetValueOnMouseOverProperty
            = DependencyProperty.RegisterAttached( "SetValueOnMouseOver", typeof( object ), typeof( MouseBehavior ),
                new FrameworkPropertyMetadata( false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SetValueOnMouseOverBindingPropertyChanged ) );

        public static void SetSetValueOnMouseOver( FrameworkElement element, object value )
        {
            element.SetValue( SetValueOnMouseOverProperty, value );
        }

        private static void SetValueOnMouseOverBindingPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            var element = d as FrameworkElement;
            if( element == null || _wiredUpElements.Contains( element ) )
            {
                return;
            }
            lock( _wiredUpElementsLock )
            {
                if( _wiredUpElements.Contains( element ) ) return;

                _wiredUpElements.Add( element );

                element.MouseEnter += ( sender, args ) =>
                {
                    var frameworkElement = (FrameworkElement)sender;
                    SetSetValueOnMouseOver( frameworkElement, true );
                };

                element.MouseLeave += ( sender, args ) =>
                {
                    var frameworkElement = (FrameworkElement)sender;
                    SetSetValueOnMouseOver( frameworkElement, false );
                };
            }
        }
    }
}

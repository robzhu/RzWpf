using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RzWpf
{
    public class CustomComboBox : ComboBox
    {
        public DataTemplate SelectionBoxTemplate
        {
            get { return (DataTemplate)GetValue( SelectionBoxTemplateProperty ); }
            set { SetValue( SelectionBoxTemplateProperty, value ); }
        }

        public static readonly DependencyProperty SelectionBoxTemplateProperty = DependencyProperty.Register(
            "SelectionBoxTemplate",
            typeof( DataTemplate ),
            typeof( CustomComboBox ),
            new FrameworkPropertyMetadata( null,
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsArrange, ( sender, e ) =>
                {
                    CustomComboBox comboBox = sender as CustomComboBox;
                    if( comboBox.selectionBoxHost == null ) return;

                    if( e.NewValue != null )
                    {
                        comboBox.selectionBoxHost.ContentTemplate = e.NewValue as DataTemplate;
                    }
                    else
                    {
                        comboBox.selectionBoxHost.ContentTemplate = comboBox.SelectionBoxItemTemplate;
                    }
                } ) );

        private ContentPresenter selectionBoxHost = null;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var contentSite = this.FindName( "ContentSite" );

            int childCount = VisualTreeHelper.GetChildrenCount( this );
            
            for( int i = 0; i < childCount; i++ )
            {
                var host = VisualTreeHelper.GetChild( this, i ) as ContentPresenter;
                if( null != host )
                {
                    selectionBoxHost = host;
                    break;
                }
            }

            if( selectionBoxHost != null )
            {
                selectionBoxHost.ContentTemplate = SelectionBoxTemplate;
            }
        }
    }
}

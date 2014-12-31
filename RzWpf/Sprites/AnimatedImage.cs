using System.Windows;
using System.Windows.Media;

namespace RzWpf
{

    //<UserControl.RenderTransform>
    //    <TransformGroup>
    //        <ScaleTransform ScaleX="{Binding ScaleX}" ScaleY="{Binding ScaleY}"/>
    //        <TranslateTransform X="{Binding OffsetX}" Y="{Binding OffsetY}"/>
    //    </TransformGroup>
    //</UserControl.RenderTransform>

    //<Image Source="{Binding Image}" Clip="{Binding Clip}"
    //       Width="{Binding Width}" Height="{Binding Height}" Opacity="{Binding Opacity}"
    //       Visibility="{Binding Visible, Converter={StaticResource HiddenIfFalseConverter}}">
    //    <Image.RenderTransform>
    //        <TransformGroup>
    //            <TranslateTransform X="{Binding ImageOffsetX}" Y="{Binding ImageOffsetY}"/>
    //        </TransformGroup>
    //    </Image.RenderTransform>
    //</Image>

    public class AnimatedImage : FrameworkElement
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register( "Source", typeof( ImageSource ), typeof( AnimatedImage ), new PropertyMetadata( null ) );

        public ImageSource Source
        {
            get { return (ImageSource)GetValue( SourceProperty ); }
            set { SetValue( SourceProperty, value ); }
        }

        public static readonly DependencyProperty ClipProperty =
            DependencyProperty.Register( "Clip", typeof( RectangleGeometry ), typeof( AnimatedImage ), new PropertyMetadata( null ) );

        public new RectangleGeometry Clip
        {
            get { return (RectangleGeometry)GetValue( ClipProperty ); }
            set { SetValue( ClipProperty, value ); }
        }

        private DrawingVisual _visual = new DrawingVisual();

        public AnimatedImage()
        {
            AddLogicalChild( _visual );
            AddVisualChild( _visual );
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild( int index )
        {
            return _visual;
        }

        protected override Size MeasureOverride( Size availableSize )
        {
            return Clip == null ? Size.Empty : new Size( Clip.Rect.Width, Clip.Rect.Height );
        }

        protected override void OnPropertyChanged( DependencyPropertyChangedEventArgs e )
        {
            base.OnPropertyChanged( e );
            if( e.Property.Name == "Source" ) UpdateSource();
            if( e.Property.Name == "Clip" ) UpdateClip();
        }

        private void UpdateSource()
        {
            if( Source != null )
            {
                var dc = _visual.RenderOpen();
                dc.DrawImage( Source, new Rect( 0, 0, Source.Width, Source.Height ) );
                dc.Close();
            }
        }

        private void UpdateClip()
        {
            _visual.Clip = Clip;
            if( Clip != null )
            {
                _visual.Transform = new TranslateTransform( -Clip.Rect.X, -Clip.Rect.Y );
            }
        }
    }
}

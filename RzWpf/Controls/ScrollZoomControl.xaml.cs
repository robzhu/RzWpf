using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RzWpf
{
    public partial class ScrollZoomControl : UserControl
    {
        public static readonly DependencyProperty ZoomLevelProperty =
            DependencyProperty.Register("ZoomLevel", typeof (double), typeof (ScrollZoomControl), new PropertyMetadata(default(double)));

        public double ZoomLevel
        {
            get { return (double) GetValue(ZoomLevelProperty); }
            set { SetValue(ZoomLevelProperty, value); }
        }

        public static readonly DependencyProperty MaxZoomLevelProperty =
            DependencyProperty.Register("MaxZoomLevel", typeof (double), typeof (ScrollZoomControl), new PropertyMetadata(default(double)));

        public double MaxZoomLevel
        {
            get { return (double) GetValue(MaxZoomLevelProperty); }
            set { SetValue(MaxZoomLevelProperty, value); }
        }

        public static readonly DependencyProperty MinZoomLevelProperty =
            DependencyProperty.Register("MinZoomLevel", typeof (double), typeof (ScrollZoomControl), new PropertyMetadata(default(double)));

        public double MinZoomLevel
        {
            get { return (double) GetValue(MinZoomLevelProperty); }
            set { SetValue(MinZoomLevelProperty, value); }
        }

        public static new readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof (object), typeof (ScrollZoomControl), new PropertyMetadata(default(object)));

        public new object Content
        {
            get { return (object) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public ScrollZoomControl()
        {
            InitializeComponent();
            PreviewMouseWheel += new MouseWheelEventHandler( ScrollZoomControlPreviewMouseWheel );
        }

        void ScrollZoomControlPreviewMouseWheel( object sender, MouseWheelEventArgs e )
        {
            e.Handled = true;
            ZoomLevel += ( (double)e.Delta / 1000 );
            ZoomLevel = Math.Min( ZoomLevel, MaxZoomLevel );
            ZoomLevel = Math.Max( ZoomLevel, MinZoomLevel );
        }
    }
}

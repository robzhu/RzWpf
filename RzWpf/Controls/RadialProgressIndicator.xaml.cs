using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RzWpf
{
    /// <summary>
    /// This control uses a radial arm whose angle defines a region that represents the current Value as a fraction of the Maximum value.
    /// </summary>
    public partial class RadialProgressIndicator : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register( "Value", typeof( double ), typeof( RadialProgressIndicator ),
            new FrameworkPropertyMetadata( 0d, new PropertyChangedCallback( RefreshIndicator ) ) );

        public double Value
        {
            get { return (double)GetValue( ValueProperty ); }
            set { SetValue( ValueProperty, value ); }
        }

        private static void RefreshIndicator( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            RadialProgressIndicator indicator = d as RadialProgressIndicator;
            if( indicator != null )
            {
                indicator.Refresh();
            }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register( "Maximum", typeof( double ), typeof( RadialProgressIndicator ),
            new FrameworkPropertyMetadata( 1d, new PropertyChangedCallback( RefreshIndicator ) ) );

        public double Maximum
        {
            get { return (double)GetValue( MaximumProperty ); }
            set { SetValue( MaximumProperty, value ); }
        }

        public static readonly DependencyProperty BarBackgroundProperty =
            DependencyProperty.Register( "BarBackground", typeof( Brush ), typeof( RadialProgressIndicator ), 
            new FrameworkPropertyMetadata( new SolidColorBrush( Colors.Transparent ) ) );

        public Brush BarBackground
        {
            get { return (Brush)GetValue( BarBackgroundProperty ); }
            set { SetValue( BarBackgroundProperty, value ); }
        }

        public static readonly DependencyProperty ValueForegroundProperty =
            DependencyProperty.Register( "ValueForeground", typeof( Brush ), typeof( RadialProgressIndicator ),
            new FrameworkPropertyMetadata( new SolidColorBrush( Colors.Gray ) ) );

        public Brush ValueForeground
        {
            get { return (Brush)GetValue( ValueForegroundProperty ); }
            set { SetValue( ValueForegroundProperty, value ); }
        }

        private static double _firstCorner = 1d / 8d;
        private static double _secondCorner = 3d / 8d;
        private static double _thirdCorner = 5d / 8d;
        private static double _fourthCorner = 7d / 8d;

        public RadialProgressIndicator()
        {
            InitializeComponent();
            _progress.Points = new PointCollection( 8 );
        }

        private void Refresh()
        {
            double ratio = 1 - ( Value / Maximum );
            PointCollection points = _progress.Points;

            points.Clear();

            double side = Math.Min( ActualHeight, ActualWidth );

            points.Add( new Point( side, side / 2 ) );
            points.Add( new Point( side / 2, side / 2 ) );

            var v = ClipToCube( side * VectorUtility.UnitVectorWithAngleRadians( ratio * 2 * Math.PI ), side / 2, new Vector( side / 2, side / 2 ) );
            var point = v.ToPoint();

            points.Add( point );

            if( ratio < _firstCorner )
            {
                points.Add( new Point( side, side ) );
                points.Add( new Point( 0, side ) );
                points.Add( new Point( 0, 0 ) );
                points.Add( new Point( side, 0 ) );
            }
            else if( ratio < _secondCorner )
            {
                points.Add( new Point( 0, side ) );
                points.Add( new Point( 0, 0 ) );
                points.Add( new Point( side, 0 ) );
            }
            else if( ratio < _thirdCorner )
            {
                points.Add( new Point( 0, 0 ) );
                points.Add( new Point( side, 0 ) );
            }
            else if( ratio < _fourthCorner )
            {
                points.Add( new Point( side, 0 ) );
            }
        }

        /// <summary>
        /// Clips the specified vector to within an origin bound cube that has sides of 2*side.
        /// </summary>
        /// <param name="vector">The vector to clip.</param>
        /// <param name="side">Half the side of the cube.</param>
        /// <returns></returns>
        private Vector ClipToCube( Vector vector, double side, Vector offset )
        {
            double clippedX, clippedY;
            double ratio = vector.X / vector.Y;

            if( Math.Abs( ratio ) > 1 )
            {
                clippedX = ( vector.X < 0 ) ? -side : side;
                clippedY = Math.Abs( side / ratio );
                if( vector.Y < 0 ) clippedY = -clippedY;
            }
            else
            {
                clippedY = ( vector.Y < 0 ) ? -side : side;
                clippedX = Math.Abs( side * ratio );
                if( vector.X < 0 ) clippedX = -clippedX;
            }

            return ( new Vector( clippedX, clippedY ) + offset );
        }
    }
}

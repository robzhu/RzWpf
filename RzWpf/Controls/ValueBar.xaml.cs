using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RzWpf
{
    public partial class ValueBar : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register( "Value", typeof( double ), typeof( ValueBar ), new FrameworkPropertyMetadata( 0d, OnValuePropertyChangedCallback ) );

        public double Value
        {
            get { return (double)GetValue( ValueProperty ); }
            set { SetValue( ValueProperty, value ); }
        }

        public static readonly DependencyProperty AnimateResidualProperty =
            DependencyProperty.Register( "AnimateResidual", typeof( bool ), typeof( ValueBar ), new FrameworkPropertyMetadata( true ) );

        public bool AnimateResidual
        {
            get { return (bool)GetValue( AnimateResidualProperty ); }
            set { SetValue( AnimateResidualProperty, value ); }
        }

        public static readonly DependencyProperty ResidualValueProperty =
            DependencyProperty.Register( "ResidualValue", typeof( double ), typeof( ValueBar ), new FrameworkPropertyMetadata( 0d ) );

        public double ResidualValue
        {
            get { return (double)GetValue( ResidualValueProperty ); }
            set { SetValue( ResidualValueProperty, value ); }
        }

        public static readonly DependencyProperty DecrementResidualBarBrushProperty =
            DependencyProperty.Register( "DecrementResidualBarBrush", typeof( Brush ), typeof( ValueBar ),
            new FrameworkPropertyMetadata( new SolidColorBrush( Colors.Transparent ) ) );

        public Brush DecrementResidualBarBrush
        {
            get { return (Brush)GetValue( DecrementResidualBarBrushProperty ); }
            set { SetValue( DecrementResidualBarBrushProperty, value ); }
        }

        public static readonly DependencyProperty IncrementResidualBarBrushProperty =
            DependencyProperty.Register( "IncrementResidualBarBrush", typeof( Brush ), typeof( ValueBar ),
            new FrameworkPropertyMetadata( new SolidColorBrush( Colors.Transparent ) ) );

        public Brush IncrementResidualBarBrush
        {
            get { return (Brush)GetValue( IncrementResidualBarBrushProperty ); }
            set { SetValue( IncrementResidualBarBrushProperty, value ); }
        }

        public static readonly DependencyProperty MaximumProperty = 
            DependencyProperty.Register( "Maximum", typeof( double ), typeof( ValueBar ), new FrameworkPropertyMetadata( 0d ) );

        public double Maximum
        {
            get { return (double)GetValue( MaximumProperty ); }
            set { SetValue( MaximumProperty, value ); }
        }

        public static readonly DependencyProperty MinimumProperty = 
            DependencyProperty.Register( "Minimum", typeof( double ), typeof( ValueBar ), new FrameworkPropertyMetadata( 0d ) );

        public double Minimum
        {
            get { return (double)GetValue( MinimumProperty ); }
            set { SetValue( MinimumProperty, value ); }
        }

        public static readonly DependencyProperty BarBackgroundProperty = 
            DependencyProperty.Register( "BarBackground", typeof( Brush ), typeof( ValueBar ), 
            new FrameworkPropertyMetadata( new SolidColorBrush( Colors.Transparent ) ) );

        public Brush BarBackground
        {
            get { return (Brush)GetValue( BarBackgroundProperty ); }
            set { SetValue( BarBackgroundProperty, value ); }
        }

        public static readonly DependencyProperty BarBorderBrushProperty =
            DependencyProperty.Register( "BarBorderBrush", typeof( Brush ), typeof( ValueBar ), 
            new FrameworkPropertyMetadata( new SolidColorBrush( Colors.White ) ) );

        public Brush BarBorderBrush
        {
            get { return (Brush)GetValue( BarBorderBrushProperty ); }
            set { SetValue( BarBorderBrushProperty, value ); }
        }

        public static readonly DependencyProperty BarBorderCornerRadiusProperty =
            DependencyProperty.Register( "BarBorderCornerRadius", typeof( double ), typeof( ValueBar ), new FrameworkPropertyMetadata( 2d ) );

        public double BarBorderCornerRadius
        {
            get { return (double)GetValue( BarBorderCornerRadiusProperty ); }
            set { SetValue( BarBorderCornerRadiusProperty, value ); }
        }

        public static readonly DependencyProperty BarBorderThicknessProperty =
            DependencyProperty.Register( "BarBorderThickness", typeof( double ), typeof( ValueBar ), new FrameworkPropertyMetadata( 2d ) );

        public double BarBorderThickness
        {
            get { return (double)GetValue( BarBorderThicknessProperty ); }
            set { SetValue( BarBorderThicknessProperty, value ); }
        }

        public static readonly DependencyProperty OverlayBrushBrushProperty =
            DependencyProperty.Register( "OverlayBrush", typeof( Brush ), typeof( ValueBar ), 
            new FrameworkPropertyMetadata( new SolidColorBrush( (Color)ColorConverter.ConvertFromString("#80FFFFFF") ) ) );

        public Brush OverlayBrush
        {
            get { return (Brush)GetValue( OverlayBrushBrushProperty ); }
            set { SetValue( OverlayBrushBrushProperty, value ); }
        }

        public ValueBar()
        {
            InitializeComponent();
        }

        private static void OnValuePropertyChangedCallback( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ValueBar vb = d as ValueBar;
            if( vb != null ) vb.OnValueChanged();
        }

        private void OnValueChanged()
        {
            if( AnimateResidual )
            {
                if( Value < ResidualValue )
                {
                    Duration duration = new Duration( TimeSpan.FromSeconds( 0.5 ) );
                    DoubleAnimation doubleanimation = new DoubleAnimation( ResidualValue, Value, duration );
                    doubleanimation.Completed += ( s, e ) => { ResidualValue = Value; };
                    BeginAnimation( ValueBar.ResidualValueProperty, doubleanimation );
                }
                else
                {
                    //animate the bar increasing
                    PART_ValueBar.Foreground = IncrementResidualBarBrush;
                    PART_IncrementResidualValueBar.Visibility = Visibility.Visible;
                    PART_IncrementResidualValueBar.Foreground = Foreground;

                    Duration duration = new Duration( TimeSpan.FromSeconds( 0.5 ) );
                    DoubleAnimation doubleanimation = new DoubleAnimation( ResidualValue, Value, duration );
                    doubleanimation.Completed += ( s, e ) =>
                    {
                        ResidualValue = Value;
                        PART_IncrementResidualValueBar.Visibility = Visibility.Hidden;
                        PART_ValueBar.Foreground = Foreground;
                    };
                    BeginAnimation( ValueBar.ResidualValueProperty, doubleanimation );
                }
            }
        }
    }
}

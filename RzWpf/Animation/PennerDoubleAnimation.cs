/**
 * PennerDoubleAnimation
 * Animates the value of a double property between two target values using 
 * Robert Penner's easing equations for interpolation over a specified Duration.
 *
 * @author		Darren David darren-code@lookorfeel.com
 * @version		1.0
 *
 * Credit/Thanks:
 * Robert Penner - The easing equations we all know and love 
 *   (http://robertpenner.com/easing/) [See License.txt for license info]
 * 
 * Lee Brimelow - initial port of Penner's equations to WPF 
 *   (http://thewpfblog.com/?p=12)
 * 
 * Zeh Fernando - additional equations (out/in) from 
 *   caurina.transitions.Tweener (http://code.google.com/p/tweener/)
 */


using RzAspects;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

namespace RzWpf
{
    /// <summary>
    /// Animates the value of a double property between two target values using 
    /// Robert Penner's easing equations for interpolation over a specified Duration.
    /// </summary>
    /// <example>
    /// <code>
    /// // C#
    /// PennerDoubleAnimation anim = new PennerDoubleAnimation();
    /// anim.Type = PennerDoubleAnimation.Equations.Linear;
    /// anim.From = 1;
    /// anim.To = 0;
    /// myControl.BeginAnimation( OpacityProperty, anim );
    /// 
    /// // XAML
    /// <Storyboard x:Key="AnimateXamlRect">
    ///  <animation:PennerDoubleAnimation 
    ///    Storyboard.TargetName="myControl" 
    ///    Storyboard.TargetProperty="(Canvas.Left)"
    ///    From="0" 
    ///    To="600" 
    ///    Equation="BackEaseOut" 
    ///    Duration="00:00:05" />
    /// </Storyboard>
    /// 
    /// <Control.Triggers>
    ///   <EventTrigger RoutedEvent="FrameworkElement.Loaded">
    ///     <BeginStoryboard Storyboard="{StaticResource AnimateXamlRect}"/>
    ///   </EventTrigger>
    /// </Control.Triggers>
    /// </code>
    /// </example>
    public class PennerDoubleAnimation : DoubleAnimationBase
    {
        #region Fields

        private Func<double, double, double, double, double> _EasingMethod = EasingFunctions.Linear;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty EquationProperty =
            DependencyProperty.Register(
                "Equation", typeof( EasingFunctionId ), typeof( PennerDoubleAnimation ),
                new PropertyMetadata( EasingFunctionId.Linear, new PropertyChangedCallback( HandleEquationChanged ) ) );

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(
                "From", typeof( double ), typeof( PennerDoubleAnimation ), new PropertyMetadata( 0.0 ) );

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(
                "To", typeof( double ), typeof( PennerDoubleAnimation ), new PropertyMetadata( 0.0 ) );

        #endregion

        #region Constructors

        public PennerDoubleAnimation()
        {
        }

        public PennerDoubleAnimation( EasingFunctionId type, double from, double to )
        {
            Equation = type;
            From = from;
            To = to;
        }

        public PennerDoubleAnimation( EasingFunctionId type, double from, double to, System.Windows.Duration duration )
            : this( type, from, to )
        {
            Duration = duration;
        }

        #endregion

        #region Abstract Member Implementations

        protected override double GetCurrentValueCore( double startValue, double targetValue, AnimationClock clock )
        {
            try
            {
                // might be able to speed this up by caching it, but who knows
                //object[] methodParams = new object[ 4 ] { clock.CurrentTime.Value.TotalSeconds, From, To - From, Duration.TimeSpan.TotalSeconds };

                return _EasingMethod( clock.CurrentTime.Value.TotalSeconds, From, To - From, Duration.TimeSpan.TotalSeconds );
                //return (double)_EasingMethod.Invoke( this, methodParams );
            }
            catch
            {
                return From;
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new PennerDoubleAnimation();
        }

        #endregion

        

        #region Event Handlers

        private static void HandleEquationChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            PennerDoubleAnimation pda = sender as PennerDoubleAnimation;

            // cache method so we avoid lookup while animating
            pda._EasingMethod = EasingFunctions.EquationToFunc( (EasingFunctionId)e.NewValue );
            //pda._EasingMethod = typeof( PennerDoubleAnimation ).GetMethod( e.NewValue.ToString() );
        }

        #endregion

        #region Properties

        /// <summary>
        /// The easing equation to use.
        /// </summary>
        [TypeConverter( typeof( PennerDoubleAnimationTypeConverter ) )]
        public EasingFunctionId Equation
        {
            get { return (EasingFunctionId)GetValue( EquationProperty ); }
            set
            {
                SetValue( EquationProperty, value );

                // cache method so we avoid lookup while animating
                _EasingMethod = EasingFunctions.EquationToFunc( value );
                //_EasingMethod = this.GetType().GetMethod( value.ToString() );
            }
        }

        /// <summary>
        /// Starting value for the animation.
        /// </summary>
        public double From
        {
            get { return (double)GetValue( FromProperty ); }
            set { SetValue( FromProperty, value ); }
        }

        /// <summary>
        /// Ending value for the animation.
        /// </summary>
        public double To
        {
            get { return (double)GetValue( ToProperty ); }
            set { SetValue( ToProperty, value ); }
        }

        #endregion
    }

    public class PennerDoubleAnimationTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
        {
            return sourceType == typeof( string );
        }

        public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
        {
            return destinationType == typeof( Enum );
        }

        public override object ConvertFrom( ITypeDescriptorContext context, CultureInfo culture, object value )
        {
            foreach( int i in Enum.GetValues( typeof( EasingFunctionId ) ) )
            {
                if( Enum.GetName( typeof( EasingFunctionId ), i ) == value.ToString() )
                    return (EasingFunctionId)i;
            }
            return null;
        }

        public override object ConvertTo( ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType )
        {
            if( value != null )
            {
                return ( (EasingFunctionId)value ).ToString();
            }
            return null;
        }
    }
}

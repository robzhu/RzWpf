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
 *   [See License.txt for license info]
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Globalization;

namespace RzWpf
{
    /// <summary>
    /// Animates the value of a int property between two target values using 
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
    public class PennerIntAnimation : Int32AnimationBase
    {
        /// <summary>
        /// Enumeration of all easing equations.
        /// </summary>
        public enum Equations
        {
            Linear,
            QuadEaseOut, QuadEaseIn, QuadEaseInOut, QuadEaseOutIn,
            ExpoEaseOut, ExpoEaseIn, ExpoEaseInOut, ExpoEaseOutIn,
            CubicEaseOut, CubicEaseIn, CubicEaseInOut, CubicEaseOutIn,
            QuartEaseOut, QuartEaseIn, QuartEaseInOut, QuartEaseOutIn,
            QuintEaseOut, QuintEaseIn, QuintEaseInOut, QuintEaseOutIn,
            CircEaseOut, CircEaseIn, CircEaseInOut, CircEaseOutIn,
            SineEaseOut, SineEaseIn, SineEaseInOut, SineEaseOutIn,
            ElasticEaseOut, ElasticEaseIn, ElasticEaseInOut, ElasticEaseOutIn,
            BounceEaseOut, BounceEaseIn, BounceEaseInOut, BounceEaseOutIn,
            BackEaseOut, BackEaseIn, BackEaseInOut, BackEaseOutIn
        }

        #region Fields

        private MethodInfo _EasingMethod;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty EquationProperty =
            DependencyProperty.Register(
                "Equation", typeof( Equations ), typeof( PennerIntAnimation ),
                new PropertyMetadata( Equations.Linear, new PropertyChangedCallback( HandleEquationChanged ) ) );

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register(
                "From", typeof( int ), typeof( PennerIntAnimation ), new PropertyMetadata( 0 ) );

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register(
                "To", typeof( int ), typeof( PennerIntAnimation ), new PropertyMetadata( 0 ) );

        #endregion

        #region Constructors

        public PennerIntAnimation()
        {
        }

        public PennerIntAnimation( Equations type, int from, int to )
        {
            Equation = type;
            From = from;
            To = to;
        }

        public PennerIntAnimation( Equations type, int from, int to, Duration duration )
        {
            Equation = type;
            From = from;
            To = to;
            Duration = duration;
        }

        #endregion

        #region Abstract Member Implementations

        protected override int GetCurrentValueCore( int startValue, int targetValue, AnimationClock clock )
        {
            try
            {
                // might be able to speed this up by caching it, but who knows
                object[] methodParams = new object[ 4 ] { clock.CurrentTime.Value.TotalSeconds, From, To - From, Duration.TimeSpan.TotalSeconds };

                return (int)_EasingMethod.Invoke( this, methodParams );
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

        #region Equations

        // These methods are all public to enable reflection in GetCurrentValueCore.

        #region Linear

        /// <summary>
        /// Easing equation function for a simple linear tweening, with no easing.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int Linear( double t, double b, double c, double d )
        {
            return (int)( c * t / d + b );
        }

        #endregion

        #region Expo

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int ExpoEaseOut( double t, double b, double c, double d )
        {
            var result = ( t == d ) ? b + c : c * ( -Math.Pow( 2, -10 * t / d ) + 1 ) + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int ExpoEaseIn( double t, double b, double c, double d )
        {
            var result = ( t == 0 ) ? b : c * Math.Pow( 2, 10 * ( t / d - 1 ) ) + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int ExpoEaseInOut( double t, double b, double c, double d )
        {
            if( t == 0 )
                return (int)b;

            if( t == d )
                return (int)( b + c );

            if( ( t /= d / 2 ) < 1 )
                return (int)( c / 2 * Math.Pow( 2, 10 * ( t - 1 ) ) + b );

            return (int)( c / 2 * ( -Math.Pow( 2, -10 * --t ) + 2 ) + b );
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int ExpoEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return ExpoEaseOut( t * 2, b, c / 2, d );

            return ExpoEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Circular

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int CircEaseOut( double t, double b, double c, double d )
        {
            return (int)( c * Math.Sqrt( 1 - ( t = t / d - 1 ) * t ) + b );
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int CircEaseIn( double t, double b, double c, double d )
        {
            var result = -c * ( Math.Sqrt( 1 - ( t /= d ) * t ) - 1 ) + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int CircEaseInOut( double t, double b, double c, double d )
        {
            if( ( t /= d / 2 ) < 1 )
            {
                var result = -c / 2 * ( Math.Sqrt( 1 - t * t ) - 1 ) + b;
                return (int)result;
            }

            var result2 = c / 2 * ( Math.Sqrt( 1 - ( t -= 2 ) * t ) + 1 ) + b;
            return (int)result2;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int CircEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return CircEaseOut( t * 2, b, c / 2, d );

            return CircEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Quad

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuadEaseOut( double t, double b, double c, double d )
        {
            var result = -c * ( t /= d ) * ( t - 2 ) + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuadEaseIn( double t, double b, double c, double d )
        {
            var result = c * ( t /= d ) * t + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuadEaseInOut( double t, double b, double c, double d )
        {
            if( ( t /= d / 2 ) < 1 )
                return (int)( c / 2 * t * t + b );

            return (int)( -c / 2 * ( ( --t ) * ( t - 2 ) - 1 ) + b );
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuadEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return QuadEaseOut( t * 2, b, c / 2, d );

            return QuadEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Sine

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int SineEaseOut( double t, double b, double c, double d )
        {
            return (int)( c * Math.Sin( t / d * ( Math.PI / 2 ) ) + b );
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int SineEaseIn( double t, double b, double c, double d )
        {
            return (int)( -c * Math.Cos( t / d * ( Math.PI / 2 ) ) + c + b );
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int SineEaseInOut( double t, double b, double c, double d )
        {
            if( ( t /= d / 2 ) < 1 )
                return (int)( c / 2 * ( Math.Sin( Math.PI * t / 2 ) ) + b );

            return (int)( -c / 2 * ( Math.Cos( Math.PI * --t / 2 ) - 2 ) + b );
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int SineEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return SineEaseOut( t * 2, b, c / 2, d );

            return SineEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Cubic

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int CubicEaseOut( double t, double b, double c, double d )
        {
            var result = c * ( ( t = t / d - 1 ) * t * t + 1 ) + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int CubicEaseIn( double t, double b, double c, double d )
        {
            var result =  c * ( t /= d ) * t * t + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int CubicEaseInOut( double t, double b, double c, double d )
        {
            if( ( t /= d / 2 ) < 1 )
                return (int)( c / 2 * t * t * t + b );

            return (int)( c / 2 * ( ( t -= 2 ) * t * t + 2 ) + b );
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int CubicEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return CubicEaseOut( t * 2, b, c / 2, d );

            return CubicEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Quartic

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuartEaseOut( double t, double b, double c, double d )
        {
            var result = -c * ( ( t = t / d - 1 ) * t * t * t - 1 ) + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuartEaseIn( double t, double b, double c, double d )
        {
            var result =  c * ( t /= d ) * t * t * t + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuartEaseInOut( double t, double b, double c, double d )
        {
            if( ( t /= d / 2 ) < 1 )
                return (int)( c / 2 * t * t * t * t + b );

            return (int)( -c / 2 * ( ( t -= 2 ) * t * t * t - 2 ) + b );
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuartEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return QuartEaseOut( t * 2, b, c / 2, d );

            return QuartEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Quintic

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuintEaseOut( double t, double b, double c, double d )
        {
            var result = c * ( ( t = t / d - 1 ) * t * t * t * t + 1 ) + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuintEaseIn( double t, double b, double c, double d )
        {
            var result = c * ( t /= d ) * t * t * t * t + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuintEaseInOut( double t, double b, double c, double d )
        {
            if( ( t /= d / 2 ) < 1 )
                return (int)( c / 2 * t * t * t * t * t + b );
            return (int)( c / 2 * ( ( t -= 2 ) * t * t * t * t + 2 ) + b );
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int QuintEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return QuintEaseOut( t * 2, b, c / 2, d );
            return QuintEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Elastic

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int ElasticEaseOut( double t, double b, double c, double d )
        {
            if( ( t /= d ) == 1 )
                return (int)( b + c );

            double p = d * .3;
            double s = p / 4;

            var result = ( c * Math.Pow( 2, -10 * t ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) + c + b );
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int ElasticEaseIn( double t, double b, double c, double d )
        {
            if( ( t /= d ) == 1 )
                return (int)( b + c );

            double p = d * .3;
            double s = p / 4;

            var result = -( c * Math.Pow( 2, 10 * ( t -= 1 ) ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) ) + b;
            return (int)result;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int ElasticEaseInOut( double t, double b, double c, double d )
        {
            if( ( t /= d / 2 ) == 2 )
                return (int)( b + c );

            double p = d * ( .3 * 1.5 );
            double s = p / 4;

            if( t < 1 )
                return (int)( -.5 * ( c * Math.Pow( 2, 10 * ( t -= 1 ) ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) ) + b );
            return (int)( c * Math.Pow( 2, -10 * ( t -= 1 ) ) * Math.Sin( ( t * d - s ) * ( 2 * Math.PI ) / p ) * .5 + c + b );
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int ElasticEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return ElasticEaseOut( t * 2, b, c / 2, d );
            return ElasticEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Bounce

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int BounceEaseOut( double t, double b, double c, double d )
        {
            if( ( t /= d ) < ( 1 / 2.75 ) )
                return (int)( c * ( 7.5625 * t * t ) + b );
            else if( t < ( 2 / 2.75 ) )
                return (int)( c * ( 7.5625 * ( t -= ( 1.5 / 2.75 ) ) * t + .75 ) + b );
            else if( t < ( 2.5 / 2.75 ) )
                return (int)( c * ( 7.5625 * ( t -= ( 2.25 / 2.75 ) ) * t + .9375 ) + b );
            else
                return (int)( c * ( 7.5625 * ( t -= ( 2.625 / 2.75 ) ) * t + .984375 ) + b );
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int BounceEaseIn( double t, double b, double c, double d )
        {
            return (int)( c - BounceEaseOut( d - t, 0, c, d ) + b );
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int BounceEaseInOut( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return (int)( BounceEaseIn( t * 2, 0, c, d ) * .5 + b );
            else
                return (int)( BounceEaseOut( t * 2 - d, 0, c, d ) * .5 + c * .5 + b );
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static int BounceEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return BounceEaseOut( t * 2, b, c / 2, d );
            return BounceEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #region Back

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseOut( double t, double b, double c, double d )
        {
            return c * ( ( t = t / d - 1 ) * t * ( ( 1.70158 + 1 ) * t + 1.70158 ) + 1 ) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseIn( double t, double b, double c, double d )
        {
            return c * ( t /= d ) * t * ( ( 1.70158 + 1 ) * t - 1.70158 ) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseInOut( double t, double b, double c, double d )
        {
            double s = 1.70158;
            if( ( t /= d / 2 ) < 1 )
                return c / 2 * ( t * t * ( ( ( s *= ( 1.525 ) ) + 1 ) * t - s ) ) + b;
            return c / 2 * ( ( t -= 2 ) * t * ( ( ( s *= ( 1.525 ) ) + 1 ) * t + s ) + 2 ) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static double BackEaseOutIn( double t, double b, double c, double d )
        {
            if( t < d / 2 )
                return BackEaseOut( t * 2, b, c / 2, d );
            return BackEaseIn( ( t * 2 ) - d, b + c / 2, c / 2, d );
        }

        #endregion

        #endregion

        #region Event Handlers

        private static void HandleEquationChanged( object sender, DependencyPropertyChangedEventArgs e )
        {
            PennerIntAnimation pda = sender as PennerIntAnimation;

            // cache method so we avoid lookup while animating
            pda._EasingMethod = typeof( PennerIntAnimation ).GetMethod( e.NewValue.ToString() );
        }

        #endregion

        #region Properties

        /// <summary>
        /// The easing equation to use.
        /// </summary>
        [TypeConverter( typeof( PennerDoubleAnimationTypeConverter ) )]
        public Equations Equation
        {
            get { return (Equations)GetValue( EquationProperty ); }
            set
            {
                SetValue( EquationProperty, value );

                // cache method so we avoid lookup while animating
                _EasingMethod = this.GetType().GetMethod( value.ToString() );
            }
        }

        /// <summary>
        /// Starting value for the animation.
        /// </summary>
        public int From
        {
            get { return (int)GetValue( FromProperty ); }
            set { SetValue( FromProperty, value ); }
        }

        /// <summary>
        /// Ending value for the animation.
        /// </summary>
        public int To
        {
            get { return (int)GetValue( ToProperty ); }
            set { SetValue( ToProperty, value ); }
        }

        #endregion
    }
}

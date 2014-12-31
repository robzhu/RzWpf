using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using MahApps.Metro.Controls;
using RzAspects;

namespace RzWpf
{
    /// <summary>
    /// Shell window that supports common functionality:
    /// -Registraters as the handler for NavigationServiceRegistry
    /// -Handles the dialog stack.
    /// -Handles forward/back navigations.
    /// </summary>
    public partial class ShellWindow : MetroWindow
    {
        private ShellWindowViewModel ViewModel { get; set; }
        private BlurEffect _blurEffect = new BlurEffect();
        private List<object> _journal = new List<object>();
        private IBucketUpdateService UpdateService { get; set; }

        public ShellWindow()
        {
            InitializeComponent();

            Closing += ( s, e ) => Navigation.RaiseClosing();
            Loaded += ( s, e ) => UpdateContentSize();
            SizeChanged += ( s, e ) => UpdateContentSize();
            _background.DataContextChanged += ( s, e ) => AlignBackground();

            DispatcherUtility.RegisterHandler( UIThread.Marshall );

            if( IoCContainer.GetInstance<IBucketUpdateService>() == null )
            {
                UpdateService = new BucketUpdateService( new WpfFrameEventProvider() );
                IoCContainer.RegisterSingle<IBucketUpdateService>( () =>
                {
                    return UpdateService;
                } );
            }

            Navigation.NavigateAction = OnNavigateForward;
            Navigation.BackAction = OnNavigateBack;
            Navigation.ShowDialogAction = OnShowDialog;
            Navigation.CloseWindowAction = () => { Close(); };

            try
            {
                ViewModel = IoCContainer.GetInstance<ShellWindowViewModel>();
                ViewModel.Window = this;
                DataContext = ViewModel;
                ViewModel.OnLoaded();
                UpdateContentSize();
            }
            catch
            {
                throw new Exception( "Could not locate an instance of ShellWindowViewModel, make sure to export this type during app initialization" );
            }
        }

        private void UpdateContentSize()
        {
            UpdateSize( _backgroundOverlay );
            UpdateSize( _contentFrame );
            UpdateSize( _contentCanvas );
            UpdateSize( _nextContentFrame );
            UpdateSize( _dialogBorder );
            UpdateSize( _dialogFrame );
            UpdateSize( _nextDialogFrame );
            AlignBackground();

            if( ViewModel.BlurBehind )
            {
                WindowExtensions.EnableBlurBehindWindow( this );
            }
        }

        private void AlignBackground()
        {
            if( ( _background.Source != null ) && ( _background.Source.Height != double.NaN ) ) //quick check to see if the image is currently valid.
            {
                //align the image's bottom left corner with the window's bottom left corner.
                double top = ActualHeight - _background.Source.Height;
                _background.SetValue( Canvas.TopProperty, top / 2 );
                _background.SetValue( Canvas.LeftProperty, 0d );
            }
        }

        private void UpdateSize( FrameworkElement control )
        {
            control.Width = this.ActualWidth;
            control.Height = Math.Max( 0, this.ActualHeight - 30d );
        }

        private async void OnNavigateBack()
        {
            if( _journal.Count < 2 ) return;
            _journal.RemoveLast();
            await AnimatePageTransition( _journal.GetLast(), false );
        }

        private async void OnNavigateForward( object targetPage )
        {
            await AnimatePageTransition( targetPage );
            _journal.Add( targetPage );
        }

        private async void OnShowDialog( IInlineDialog dialog, Action callback )
        {
            dialog.DialogClosedCallback += async ( result ) =>
            {
                if( ViewModel.DialogCount == 1 )
                {
                    await AnimateDismissDialog( ViewModel.VerticalParallax, ViewModel.AnimateDuration );
                    ViewModel.PopLastDialog();
                    if( null != callback ) callback();
                }
                else if( ViewModel.DialogCount > 1 )
                {
                    var previousDialog = ViewModel.GetPreviousDialog();
                    await AnimateDialogTransition( previousDialog, false );
                    ViewModel.PopLastDialog();
                    if( null != callback ) callback();
                }
            };

            await AnimateDialogTransition( dialog );
            ViewModel.PushDialog( dialog );
            _dialogFrame.Focus();
        }

        private async Task AnimatePageTransition( object targetPage, bool forwardNavigation = true )
        {
            //When navigating forward, the current frame slides toward the left and is replaced by the next frame sliding in from the right.
            double parallaxDistance = ViewModel.HorizontalParallax;
            double currentFrameTargetLeftMargin = -Width;
            double nextFrameCanvasLeftFrom = Width;

            if( !forwardNavigation )
            {
                //When navigating backward, the current frame slides toward the right and is replaced by the next frame sliding in from the left.
                parallaxDistance = -ViewModel.HorizontalParallax;
                currentFrameTargetLeftMargin = Width;
                nextFrameCanvasLeftFrom = -Width;
            }

            _nextContentFrame.SetValue( Canvas.LeftProperty, nextFrameCanvasLeftFrom );
            _nextContentFrame.Opacity = 0;
            _nextContentFrame.Content = targetPage;

            await AnimatePageTransition( currentFrameTargetLeftMargin, nextFrameCanvasLeftFrom, parallaxDistance );

            _nextContentFrame.Opacity = 0;
            _nextContentFrame.Content = null;

            _contentFrame.Content = targetPage;
            _contentFrame.Opacity = 1;
            _contentFrame.SetValue( Canvas.LeftProperty, 0d );
            _contentFrame.Focus();
        }

        private async Task AnimatePageTransition(
            double currentFrameTargetLeftMargin,
            double nextFrameCanvasLeftFrom,
            double parallaxDistance,
            double duration = ShellWindowViewModel.DefaultAnimateDuration,
            EasingFunctionId fn = EasingFunctionId.QuadEaseIn )
        {
            //Slide the current page off screen to the right
            var currentFrameSlideOut = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _contentFrame.SetValue( Canvas.LeftProperty, value );
                }, 0, currentFrameTargetLeftMargin, duration, fn );

            //Fade the current page out
            var currentFrameFadeOut = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _contentFrame.Opacity = value;
                }, 1, 0, duration / 2, fn );

            //Slide the target page onto the screen
            var nextFrameSlideIn = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _nextContentFrame.SetValue( Canvas.LeftProperty, value );
                }, (double)_nextContentFrame.GetValue( Canvas.LeftProperty ), 0, duration, fn );

            //Fade the next page in
            var nextFrameFadeIn = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _nextContentFrame.Opacity = value;
                }, 0, 1, duration, fn );

            //Animate the background to create a 3d parallax effect.
            var scrollBackground = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _background.SetValue( Canvas.LeftProperty, value );
                },
                (double)_background.GetValue( Canvas.LeftProperty ),                    //From its current position in the canvas
                (double)_background.GetValue( Canvas.LeftProperty ) - parallaxDistance, //animate the parallaxDistance.
                duration, fn );

            await Task.WhenAll( currentFrameSlideOut, currentFrameFadeOut,
                                nextFrameSlideIn, nextFrameFadeIn,
                                scrollBackground );
        }

        private async Task AnimateDialogTransition( IInlineDialog dialog, bool forwardNavigation = true )
        {
            //When navigating forward, the current dialog slides downward and is replaced by the next dialog sliding in from the top.
            double parallaxDistance = ViewModel.VerticalParallax;
            double currentFrameCanvasTopEndValue = Height;
            double nextFrameCanvasTopStartValue = -Height;

            if( !forwardNavigation )
            {
                //When popping a dialog, the current dialog slides up and is replaced by the previous dialog sliding in from the bottom.
                parallaxDistance = -ViewModel.VerticalParallax;
                currentFrameCanvasTopEndValue = -Height;
                nextFrameCanvasTopStartValue = Height;
            }

            _nextDialogFrame.SetValue( Canvas.TopProperty, nextFrameCanvasTopStartValue );
            _nextDialogFrame.Opacity = 1;
            _nextDialogFrame.Content = dialog;

            bool darkenAndBlur = ( ViewModel.DialogCount == 0 );

            await AnimateDialogTransition( currentFrameCanvasTopEndValue, nextFrameCanvasTopStartValue, parallaxDistance, darkenAndBlur, ViewModel.AnimateDuration );

            _nextDialogFrame.Opacity = 0;
            _nextDialogFrame.Content = null;

            _dialogFrame.Content = dialog;
            _dialogFrame.Opacity = 1;
            _dialogFrame.SetValue( Canvas.TopProperty, 0d );
        }

        private async Task AnimateDialogTransition(
            double currentFrameCanvasTopEndValue,
            double nextFrameCanvasTopStartValue,
            double parallaxDistance,
            bool darkenAndBlur,
            double duration,
            EasingFunctionId fn = EasingFunctionId.QuadEaseIn )
        {
            var animationTasks = new List<Task>();

            if( darkenAndBlur )
            {
                _dialogBorder.Visibility = Visibility.Visible;
                _contentFrame.Effect = _blurEffect;
                _background.Effect = _blurEffect;
                var blur = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _blurEffect.Radius = value;
                }, 0, ViewModel.BlurRadius, duration, fn );

                var darken = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _dialogBorder.Opacity = value;
                }, 0, 1, duration, fn );

                animationTasks.AddRange( new[] { blur, darken } );
            }

            //Slide the current page off screen to the bottom
            var currentDialogSlideOut = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _dialogFrame.SetValue( Canvas.TopProperty, value );
                }, 0, currentFrameCanvasTopEndValue, duration, fn );

            //Fade the current dialog out
            var currentFrameFadeOut = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _dialogFrame.Opacity = value;
                }, 1, 0, duration, fn );

            //Slide the next dialog onto the screen
            var nextFrameSlideIn = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _nextDialogFrame.SetValue( Canvas.TopProperty, value );
                }, (double)_nextDialogFrame.GetValue( Canvas.TopProperty ), 0, duration, fn );

            //Fade the next dialog in
            var nextFrameFadeIn = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _nextDialogFrame.Opacity = value;
                }, 0, 1, duration, fn );

            //Animate the background to create a 3d parallax effect.
            var scrollBackground = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _background.SetValue( Canvas.TopProperty, value );
                },
                (double)_background.GetValue( Canvas.TopProperty ),                    //From its current position in the canvas
                (double)_background.GetValue( Canvas.TopProperty ) - parallaxDistance, //animate the parallaxDistance.
                duration, fn );

            animationTasks.AddRange( new[] { currentDialogSlideOut, currentFrameFadeOut,
                                             nextFrameSlideIn, nextFrameFadeIn,
                                             scrollBackground } );

            await Task.WhenAll( animationTasks );
        }

        private async Task AnimateDismissDialog( double parallaxDistance, double duration, EasingFunctionId fn = EasingFunctionId.QuadEaseIn )
        {
            var animationTasks = new List<Task>();

            var unblur = PropertyAnimator.AnimateAsync(
            ( value ) =>
            {
                _blurEffect.Radius = value;
            }, ViewModel.BlurRadius, 0, duration, fn );

            var undarken = PropertyAnimator.AnimateAsync(
            ( value ) =>
            {
                _dialogBorder.Opacity = value;
            }, 1, 0, duration, fn );

            //Slide the current page off screen to the top
            var currentDialogSlideUp = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _dialogFrame.SetValue( Canvas.TopProperty, value );
                }, 0, -_dialogFrame.Height, duration, fn );

            //Fade the current dialog out
            var currentFrameFadeOut = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _dialogFrame.Opacity = value;
                }, 1, 0, duration, fn );

            //Animate the background to create a 3d parallax effect.
            var scrollBackground = PropertyAnimator.AnimateAsync(
                ( value ) =>
                {
                    _background.SetValue( Canvas.TopProperty, value );
                },
                (double)_background.GetValue( Canvas.TopProperty ),                    //From its current position in the canvas
                (double)_background.GetValue( Canvas.TopProperty ) + parallaxDistance, //animate the parallaxDistance.
                duration, fn );

            animationTasks.AddRange( new[] { unblur, undarken, currentDialogSlideUp, currentFrameFadeOut } );

            await Task.WhenAll( animationTasks );

            _contentFrame.Effect = null;
            _background.Effect = null;
            _dialogBorder.Visibility = Visibility.Hidden;

            _dialogFrame.Content = null;
            _dialogFrame.Opacity = 0;
        }
    }
}

using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using RzAspects;

namespace RzWpf
{
    public class ShellWindowViewModel : CompositePropertyChangeNotificationBase
    {
        public const double DefaultHorizontalParallax = 50;
        public const double DefaultVerticalParallax = 50;
        public const double DefaultBlurRadius = 25;
        public const double DefaultAnimateDuration = 400;

        public double HorizontalParallax { get; protected set; }
        public double VerticalParallax { get; protected set; } 
        public double BlurRadius { get; protected set; } 
        public double AnimateDuration { get; protected set; } 

        public ShellWindow Window { get; set; }

        #region Commands
        public string PropertyHelpCommand { get { return "HelpCommand"; } }
        private ICommand _HelpCommand;
        public ICommand HelpCommand
        {
            get { return _HelpCommand; }
            set { SetProperty( PropertyHelpCommand, ref _HelpCommand, value ); }
        }

        public string PropertyCloseCommand { get { return "CloseCommand"; } }
        private ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get { return _CloseCommand; }
            set { SetProperty( PropertyCloseCommand, ref _CloseCommand, value ); }
        }

        public string PropertyEscapeCommand { get { return "EscapeCommand"; } }
        private ICommand _EscapeCommand;
        public ICommand EscapeCommand
        {
            get { return _EscapeCommand; }
            set { SetProperty( PropertyEscapeCommand, ref _EscapeCommand, value ); }
        }

        public string PropertyMinimizeCommand { get { return "MinimizeCommand"; } }
        private ICommand _MinimizeCommand;
        public ICommand MinimizeCommand
        {
            get { return _MinimizeCommand; }
            set { SetProperty( PropertyMinimizeCommand, ref _MinimizeCommand, value ); }
        }

        public string PropertySettingsCommand { get { return "SettingsCommand"; } }
        private ICommand _SettingsCommand;
        public ICommand SettingsCommand
        {
            get { return _SettingsCommand; }
            set { SetProperty( PropertySettingsCommand, ref _SettingsCommand, value ); }
        }

        public string PropertyMenuCommand { get { return "MenuCommand"; } }
        private ICommand _MenuCommand;
        public ICommand MenuCommand
        {
            get { return _MenuCommand; }
            set { SetProperty( PropertyMenuCommand, ref _MenuCommand, value ); }
        }
        #endregion Commands

        public string PropertyIcon { get { return "Icon"; } }
        private string _Icon;
        public string Icon
        {
            get { return _Icon; }
            set { SetProperty( PropertyIcon, ref _Icon, value ); }
        }

        public string PropertyBackground { get { return "Background"; } }
        private string _Background;
        public string Background
        {
            get { return _Background; }
            set { SetProperty( PropertyBackground, ref _Background, value ); }
        }

        public string PropertyTitle { get { return "Title"; } }
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty( PropertyTitle, ref _Title, value ); }
        }

        public string PropertyOverlayColor { get { return "OverlayColor"; } }
        private Color _OverlayColor = (Color)ColorConverter.ConvertFromString( "#80000000" );
        public Color OverlayColor
        {
            get { return _OverlayColor; }
            set { SetProperty( PropertyOverlayColor, ref _OverlayColor, value ); }
        }

        public string PropertyBgColor { get { return "BgColor"; } }
        private Brush _BgColor;
        public Brush BgColor
        {
            get { return _BgColor; }
            set { SetProperty( PropertyBgColor, ref _BgColor, value ); }
        }

        private object _currentPage;
        public object CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty( "CurrentPage", ref _currentPage, value ); }
        }

        public string PropertyWindowState { get { return "WindowState"; } }
        private WindowState _WindowState;
        public WindowState WindowState
        {
            get { return _WindowState; }
            set { SetProperty( PropertyWindowState, ref _WindowState, value ); }
        }

        public string PropertyDialog { get { return "Dialog"; } }
        private object _Dialog = null;
        public object Dialog
        {
            get { return _Dialog; }
            private set { SetProperty( PropertyDialog, ref _Dialog, value ); }
        }

        private List<IInlineDialog> DialogStack { get; set; }
        public int DialogCount { get { return DialogStack.Count; } }
        public bool BlurBehind { get; protected set; }

        public ShellWindowViewModel()
        {
            HorizontalParallax = DefaultHorizontalParallax;
            VerticalParallax = DefaultVerticalParallax;
            BlurRadius = DefaultBlurRadius;
            AnimateDuration = DefaultAnimateDuration;

            DialogStack = new List<IInlineDialog>();

            CloseCommand = new DelegatedCommand( OnExecuteClose );
            MinimizeCommand = new DelegatedCommand( OnExecuteMinimize );
            BgColor = new SolidColorBrush( Colors.Black );
        }

        protected void DisableParallax()
        {
            HorizontalParallax = 0;
            VerticalParallax = 0;
        }

        public IInlineDialog GetPreviousDialog()
        {
            if( DialogCount <= 1 ) return null;

            return DialogStack[ DialogStack.Count - 2 ];
        }

        public void PopLastDialog()
        {
            //removes the last dialog from the list
            if( DialogStack.Count > 0 )
            {
                DialogStack.RemoveAt( DialogStack.Count - 1 );

                if( DialogStack.Count > 0 )
                {
                    Dialog = DialogStack[ DialogStack.Count - 1 ];
                }
                else
                {
                    Dialog = null;
                }
            }
        }

        public void PushDialog( IInlineDialog dialog )
        {
            DialogStack.Add( dialog );
            Dialog = dialog;
        }

        private void OnExecuteClose()
        {
            if( Window != null ) Window.Close();
        }

        private void OnExecuteMinimize()
        {
            if( Window != null ) Window.WindowState = WindowState.Minimized;
        }

        public virtual void OnLoaded() { }
    }
}

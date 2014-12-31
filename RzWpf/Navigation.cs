using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace RzWpf
{
    public static class Navigation
    {
        public delegate void DialogDelegate( IInlineDialog dialog, Action callback );

        public static Action<object> NavigateAction { get; set; }
        public static Action CloseWindowAction { get; set; }
        public static Action BackAction { get; set; }
        public static DialogDelegate ShowDialogAction { get; set; }

        public static event Action Closing;

        public static void GoTo( object targetPage )
        {
            Application.Current.Dispatcher.CheckedInvoke( () =>
            {
                if( null != NavigateAction ) NavigateAction( targetPage );
            } );
        }

        public static void RaiseClosing()
        {
            if( Closing != null ) Closing();
        }

        public static void Close()
        {
            Application.Current.Dispatcher.CheckedInvoke( () =>
            {
                if( Closing != null ) Closing();
                if( null != CloseWindowAction ) CloseWindowAction();
            } );
        }

        public static void GoBack()
        {
            Application.Current.Dispatcher.CheckedInvoke( () =>
            {
                if( null != BackAction ) BackAction();
            } );
        }

        public static void ShowDialog( IInlineDialog dialog, Action callback = null )
        {
            Debug.Assert( null != dialog );

            Application.Current.Dispatcher.CheckedInvoke( () =>
            {
                if( null != ShowDialogAction ) ShowDialogAction( dialog, callback );
            } );

            CommandManager.InvalidateRequerySuggested();
        }
    }
}

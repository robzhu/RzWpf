using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace RzWpf
{
    public static class StoryboardManager
    {
        public static DependencyProperty IDProperty =
            DependencyProperty.RegisterAttached(
                "ID",
                typeof( string ),
                typeof( StoryboardManager ),
                new PropertyMetadata( null, IDChanged ) );

        private static Dictionary<string, Storyboard> _storyboards = new Dictionary<string, Storyboard>();
        private static Dictionary<string, Queue<Action>> _storyboardsCompletedHandlers = new Dictionary<string, Queue<Action>>();

        private static void IDChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
        {
            Storyboard sb = obj as Storyboard;

            if( sb == null )
                return;

            var key = e.NewValue as string;

            EventHandler completedHandler =
                ( sender, args ) =>
                {
                    if( !_storyboardsCompletedHandlers[ key ].Any() )
                        return;

                    var action = _storyboardsCompletedHandlers[ key ].Dequeue();

                    if( action != null )
                        action();
                };

            if( !_storyboards.ContainsKey( key ) )
            {
                try
                {
                    sb.Completed += completedHandler;
                }
                catch( Exception ex )
                {
                    throw new ApplicationException( "Cannot register completed event handler for the story board. Is it frozen?", ex );
                }

                _storyboards.Add( key, sb );
                _storyboardsCompletedHandlers.Add( key, new Queue<Action>() );
            }
        }

        public static void PlayStoryboard( string id, Action onCompleted = null )
        {
            if( !_storyboards.ContainsKey( id ) )
            {
                throw new ApplicationException( "Storyboard with the given ID is not registered." );
            }

            _storyboardsCompletedHandlers[ id ].Enqueue( onCompleted );
            _storyboards[ id ].Begin();
        }

        public static void SetID( DependencyObject obj, string id )
        {
            obj.SetValue( IDProperty, id );
        }

        public static string GetID( DependencyObject obj )
        {
            return obj.GetValue( IDProperty ) as string;
        }
    }
}

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RzWpf
{
    /// <summary>
    /// This command uses delegates for both its Execute and CanExecute implementations.  
    /// Also known as a "Relay Command".
    /// </summary>
    public sealed class DelegatedCommand : ICommand
    {
        private static void IgnoredCallback() { }
        public static DelegatedCommand CreateAwaitableCommand()
        {
            return new DelegatedCommand( IgnoredCallback );
        }

        public event Action Executed;
        public async Task WhenExecutedAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            Action handler = () => tcs.TrySetResult( true );

            try
            {
                Executed += handler;
                await tcs.Task;
            }
            finally
            {
                Executed -= handler;
            }
        }

        readonly Func<bool> _canExecuteMethod = null;
        readonly Action _executeMethod = null;

        //Listen for RequerySuggested so this command responds appropriately to CommandManager.InvalidateRequerySuggested 
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if( _canExecuteMethod != null )
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if( _canExecuteMethod != null )
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Creates a DelegatedCommand using the specified exeuction delegate.  Since only an execution method is 
        /// provided, this delegated command will always return true for CanExecute.
        /// </summary>
        /// <param name="executeMethod"></param>
        public DelegatedCommand( Action executeMethod )
            : this( executeMethod, null )
        {
        }

        /// <summary>
        /// Creates a DelegatedCommand using the specified Execute and CanExecute delegates.  
        /// </summary>
        /// <param name="executeMethod">The action to perform when Execute is invoked.</param>
        /// <param name="canExecuteMethod">The predicate that determines if execution can occur.</param>
        public DelegatedCommand( Action executeMethod, Func<bool> canExecuteMethod )
        {
            if( executeMethod == null )
            {
                throw new ArgumentNullException( "executeMethod", "Execute delegate cannot be null" );
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        #region ICommand
        public bool CanExecute( object parameter )
        {
            if( _canExecuteMethod == null )
            {
                return true;
            }
            else
            {
                return _canExecuteMethod();
            }
        }

        public void Execute( object parameter )
        {
            if( _executeMethod != null )
            {
                _executeMethod();
                if( Executed != null ) Executed();
            }
        }
        #endregion
    }

    public sealed class DelegatedCommand<T> : ICommand
    {
        private static void IgnoredCallback( T arg ) { }
        public static DelegatedCommand<T> CreateAwaitableCommand()
        {
            return new DelegatedCommand<T>( IgnoredCallback );
        }

        public event Action<T> Executed;
        public async Task<T> WhenExecutedAsync()
        {
            var tcs = new TaskCompletionSource<T>();
            Action<T> handler = ( arg ) => tcs.TrySetResult( arg );

            try
            {
                Executed += handler;
                return await tcs.Task;
            }
            finally
            {
                Executed -= handler;
            }
        }

        readonly Predicate<T> _canExecuteMethod = null;
        readonly Action<T> _executeMethod = null;

        //Listen for RequerySuggested so this command responds appropriately to CommandManager.InvalidateRequerySuggested 
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if( _canExecuteMethod != null )
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if( _canExecuteMethod != null )
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Creates a DelegatedCommand using the specified exeuction delegate.  Since only an execution method is 
        /// provided, this delegated command will always return true for CanExecute.
        /// </summary>
        /// <param name="executeMethod"></param>
        public DelegatedCommand( Action<T> executeMethod )
            : this( executeMethod, null )
        {
        }

        /// <summary>
        /// Creates a DelegatedCommand using the specified Execute and CanExecute delegates.  
        /// </summary>
        /// <param name="executeMethod">The action to perform when Execute is invoked.</param>
        /// <param name="canExecuteMethod">The predicate that determines if execution can occur.</param>
        public DelegatedCommand( Action<T> executeMethod, Predicate<T> canExecuteMethod )
        {
            if( executeMethod == null )
            {
                throw new ArgumentNullException( "executeMethod", "Execute delegate cannot be null" );
            }

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }

        #region ICommand
        public bool CanExecute( Object parameter )
        {
            if( _canExecuteMethod == null )
            {
                return true;
            }
            else
            {
                return _canExecuteMethod( (T)parameter );
            }
        }

        public void Execute( Object parameter )
        {
            if( null != _executeMethod )
            {
                T arg = (T)parameter;
                _executeMethod( arg );
                if( Executed != null ) Executed( arg );
            }
        }
        #endregion
    }
}

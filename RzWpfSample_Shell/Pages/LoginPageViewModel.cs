using System.Threading;
using System.Windows.Input;
using RzAspects;
using RzWpf;

namespace RzWpfSample_Shell
{
    public class LoginPageViewModel : CompositePropertyChangeNotificationBase
    {
        public AsyncDelegateCommand LoginCommand { get; private set; }

        public LoginPageViewModel()
        {
            LoginCommand = new AsyncDelegateCommand( OnExecuteLogin );
        }

        private void OnExecuteLogin()
        {
            //Navigation.GoTo( new ContentPageViewModel() );
            //LoginCommand.Execute( 
            Thread.Sleep( 5000 );
        }

        //private bool CanExecuteLogin()
        //{

        //}
    }
}

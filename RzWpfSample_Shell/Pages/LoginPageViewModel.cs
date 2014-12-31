using System.Windows.Input;
using RzAspects;
using RzWpf;

namespace RzWpfSample_Shell
{
    public class LoginPageViewModel : CompositePropertyChangeNotificationBase
    {
        public ICommand LoginCommand { get; private set; }

        public LoginPageViewModel()
        {
            LoginCommand = new DelegatedCommand( OnExecuteLogin );
        }

        private void OnExecuteLogin()
        {
            Navigation.GoTo( new ContentPageViewModel() );
        }
    }
}

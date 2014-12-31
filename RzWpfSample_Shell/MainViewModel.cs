using System.Windows.Media;
using RzWpf;

namespace RzWpfSample_Shell
{
    public class MainViewModel : ShellWindowViewModel
    {
        public MainViewModel()
        {
            Background = "pack://application:,,,/RzWpfSample_Shell;component/Resources/Background.jpg";
            Icon = "pack://application:,,,/RzWpfSample_Shell;component/Resources/icon.png";
            Title = "Shell Window Sample";
            BgColor = new SolidColorBrush( Colors.Black );
            BlurBehind = false;
        }

        public override void OnLoaded() 
        {
            Navigation.GoTo( new LoginPageViewModel() );
        }
    }
}

using System.Windows;
using RzAspects;
using RzWpf;

namespace RzWpfSample_Shell
{
    public partial class App : Application
    {
        MainViewModel _viewModel = new MainViewModel();
        public App()
        {
            IoCContainer.RegisterSingle<ShellWindowViewModel>( () => { return _viewModel; } );
        }
    }
}

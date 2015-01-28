using System.Windows.Input;
using RzAspects;
using RzWpf;

namespace RzWpfSample_Shell
{
    public class ContentPageViewModel : CompositePropertyChangeNotificationBase
    {
        public ICommand BackCommand { get; private set; }
        public ICommand DialogCommand { get; private set; }
        public string Text { get; private set; }

        public ContentPageViewModel()
        {
            BackCommand = new DelegateCommand( OnExecuteBack );
            DialogCommand = new DelegateCommand( OnExecuteDialog );
            Text = Resources.Resources.Lorem;
        }

        private void OnExecuteBack()
        {
            Navigation.GoBack();
        }

        private void OnExecuteDialog()
        {
            var dialog = new MessageDialogViewModel()
            {
                Title = "Test",
                Message = "hello",
            };
            dialog.Show();
        }
    }
}

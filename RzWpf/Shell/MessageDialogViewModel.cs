using System.Windows.Input;
using RzWpf;

namespace RzWpf
{
    public class MessageDialogViewModel : InlineDialog
    {
        public string PropertyMessage { get { return "Message"; } }
        private string _Message;
        public string Message
        {
            get { return _Message; }
            set { SetProperty( PropertyMessage, ref _Message, value ); }
        }

        public MessageDialogViewModel( string message = null )
        {
            Message = message;
        }
    }
}

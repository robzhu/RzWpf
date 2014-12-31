using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RzWpf
{
    public interface IInlineDialog
    {
        string Title { get; }
        string CancelText { get; }
        string AcceptText { get; }

        event Action<bool> DialogClosedCallback;
        Task<bool> DialogClosedAsync();
        
        ICommand CancelCommand { get; }
        ICommand AcceptCommand { get; }
    }

    public abstract class InlineDialog : ViewModelBase, IInlineDialog
    {
        public string PropertyTitle { get { return "Title"; } }
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty( PropertyTitle, ref _Title, value ); }
        }

        public string PropertyCancelText { get { return "CancelText"; } }
        private string _CancelText = "Cancel";
        public string CancelText
        {
            get { return _CancelText; }
            set { SetProperty( PropertyCancelText, ref _CancelText, value ); }
        }

        public string PropertyAcceptText { get { return "AcceptText"; } }
        private string _AcceptText = "Accept";
        public string AcceptText
        {
            get { return _AcceptText; }
            set { SetProperty( PropertyAcceptText, ref _AcceptText, value ); }
        }

        public ICommand AcceptCommand { get; protected set; }
        public ICommand CancelCommand { get; protected set; }

        /// <summary>
        /// This event fires when the dialog is closed. Returns true if the user clicked "accept", false if "cancel"
        /// </summary>
        public event Action<bool> DialogClosedCallback;

        private TaskCompletionSource<bool> _dialogClosedCompletionSource = new TaskCompletionSource<bool>();

        public InlineDialog()
        {
            CancelCommand = new DelegatedCommand( () => Close( false ), CanExecuteCancel );
            AcceptCommand = new DelegatedCommand( () => Close( true ), CanExecuteAccept );
        }

        protected virtual bool CanExecuteCancel()
        {
            return true;
        }

        protected virtual bool CanExecuteAccept()
        {
            return true;
        }

        protected virtual void OnCancel() { }
        protected virtual void OnAccept() { }

        public InlineDialog Show()
        {
            Navigation.ShowDialog( this );
            return this;
        }

        public void Close( bool accepted = true )
        {
            OnAccept();
            if( DialogClosedCallback != null ) DialogClosedCallback( accepted );
        }
    
        public async Task<bool> DialogClosedAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            Action<bool> handler = (result) => tcs.TrySetResult( result );

            try
            {
                DialogClosedCallback += handler;
                return await tcs.Task;
            }
            finally
            {
                DialogClosedCallback -= handler;
            }
        }
    }
}

using System.Windows.Input;

namespace RzWpf
{
    public static class CommandManagerHelper
    {
        public static void InvalidateRequerySuggestedAndProcess()
        {
            CommandManager.InvalidateRequerySuggested();
            DispatcherHelper.ProcessWorkItems();
        }
    }
}

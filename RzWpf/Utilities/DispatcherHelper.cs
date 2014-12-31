using System.Windows.Threading;

namespace RzWpf
{
    public static class DispatcherHelper
    {
        private static DispatcherOperationCallback _exitFrameCallback = ExitFrame;

        /// <summary>
        /// Synchronously processes all work items in the current dispatcher queue.
        /// </summary>
        /// <param name="minimumPriority">
        /// The minimum priority. All work items of equal or higher priority will be processed.
        /// </param>
        public static void ProcessWorkItems( DispatcherPriority minimumPriority = DispatcherPriority.Background )
        {
            var frame = new DispatcherFrame();

            Dispatcher.CurrentDispatcher.BeginInvoke( minimumPriority, _exitFrameCallback, frame );

            //Force the work item to run, all queued work items of equal or higher priority will be run first. 
            Dispatcher.PushFrame( frame );
        }

        private static object ExitFrame( object state )
        {
            var frame = (DispatcherFrame)state;

            //Stops processing of work items, causing PushFrame to return.
            frame.Continue = false;
            return null;
        }
    }
}

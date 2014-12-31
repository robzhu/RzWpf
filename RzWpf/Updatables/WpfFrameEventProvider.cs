using System.Windows.Media;
using RzAspects;

namespace RzWpf
{
    public class WpfFrameEventProvider : UpdateEventSource
    {
        public WpfFrameEventProvider()
        {
            CompositionTarget.Rendering += OnUpdate;
        }

        protected override UpdateTime CreateUpdateEvent()
        {
            long elapsed = _stopwatch.ElapsedMilliseconds - _lastUpdateTime;
            return new UpdateTime()
            {
                ElapsedTime = elapsed,
                TotalTime = _stopwatch.ElapsedMilliseconds,
            };
        }
    }
}

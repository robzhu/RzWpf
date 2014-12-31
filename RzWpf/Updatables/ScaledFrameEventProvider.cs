using System.Diagnostics;
using System.Windows.Media;
using RzAspects;

namespace RzWpf
{
    public class ScaledFrameEventProvider : UpdateEventSource
    {
        public double Scale { get; private set; }

        public ScaledFrameEventProvider( double scale = 1 )
        {
            Debug.Assert( scale > 0 );
            Scale = scale;
            Paused = true;
            CompositionTarget.Rendering += OnUpdate;
        }

        protected override UpdateTime CreateUpdateEvent()
        {
            long elapsed = _stopwatch.ElapsedMilliseconds - _lastUpdateTime;
            return new UpdateTime()
            {
                ElapsedTime = ( elapsed * Scale ),
                TotalTime = _stopwatch.ElapsedMilliseconds,
            };
        }
    }
}

using System;
using System.Diagnostics;
using System.Windows.Media;

namespace RzWpf
{
    public struct WpfFrameTime
    {
        /// <summary>
        /// The time elapsed since the last update.
        /// </summary>
        public long ElapsedTime;

        /// <summary>
        /// The time elapsed since the app started.
        /// </summary>
        public long TotalTime;
    }

    public delegate void WpfFrameRenderEventHandler( WpfFrameTime frameTime );

    public static class WpfFrameTimeService
    {
        public static event WpfFrameRenderEventHandler OnGameTimeTick;

        /// <summary>
        /// The game time.
        /// </summary>
        public static WpfFrameTime FrameTime { get; private set; }

        private static Stopwatch _stopwatch = new Stopwatch();
        private static long _lastRenderTime = 0;

        static WpfFrameTimeService()
        {
            _stopwatch.Start();
            CompositionTarget.Rendering += OnRender;
        }

        private static void OnRender( object sender, EventArgs e )
        {
            long elapsed = _stopwatch.ElapsedMilliseconds - _lastRenderTime;

            if( OnGameTimeTick != null )
            {
                OnGameTimeTick( new WpfFrameTime()
                {
                    ElapsedTime = elapsed, 
                    TotalTime = _stopwatch.ElapsedMilliseconds,
                } );
            }

            _lastRenderTime = _stopwatch.ElapsedMilliseconds;
        }
    }
}

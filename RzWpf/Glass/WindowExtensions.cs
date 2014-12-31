using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace RzWpf
{
    public static class WindowExtensions
    {
        [Flags]
        private enum DwmBlurBehindFlags : uint
        {
            /// <summary>
            /// Indicates a value for fEnable has been specified.
            /// </summary>
            DWM_BB_ENABLE = 0x00000001,

            /// <summary>
            /// Indicates a value for hRgnBlur has been specified.
            /// </summary>
            DWM_BB_BLURREGION = 0x00000002,

            /// <summary>
            /// Indicates a value for fTransitionOnMaximized has been specified.
            /// </summary>
            DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004
        }

        [StructLayout( LayoutKind.Sequential )]
        private struct DWM_BLURBEHIND
        {
            public DwmBlurBehindFlags dwFlags;
            public bool fEnable;
            public IntPtr hRgnBlur;
            public bool fTransitionOnMaximized;
        }

        private static class Win32Glass
        {
            [DllImport( "dwmapi.dll" )]
            public static extern IntPtr DwmEnableBlurBehindWindow( IntPtr hWnd, ref DWM_BLURBEHIND pBlurBehind );
        }

        public static void EnableBlurBehindWindow( this Window window )
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper( window );
            IntPtr myHwnd = windowInteropHelper.Handle;

            if( myHwnd != IntPtr.Zero )
            {
                HwndSource mainWindowSrc = System.Windows.Interop.HwndSource.FromHwnd( myHwnd );

                mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb( 0, 0, 0, 0 );

                DWM_BLURBEHIND blurBehindParameters = new DWM_BLURBEHIND();
                blurBehindParameters.dwFlags = DwmBlurBehindFlags.DWM_BB_ENABLE;
                blurBehindParameters.fEnable = true;
                blurBehindParameters.hRgnBlur = IntPtr.Zero;

                Win32Glass.DwmEnableBlurBehindWindow( myHwnd, ref blurBehindParameters );
            }
        }
    }
}

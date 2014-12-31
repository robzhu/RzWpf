using System.Diagnostics;
using System.Windows;
using RzAspects;

namespace RzWpf
{
    public static class ApplicationExtensions
    {
        public static void VerifyDotNet45( this Application app )
        {
            if( !NetFXUtil.HasDotNet45() )
            {
                MessageBoxResult result = MessageBox.Show( "This application needs the .net Framework 4.5.  Click OK to open the download page.", "Oops!", MessageBoxButton.OKCancel );
                if( result == MessageBoxResult.OK )
                {
                    Process.Start( "http://www.microsoft.com/en-us/download/details.aspx?id=30653" );
                }
                app.Shutdown();
            }
        }
    }
}

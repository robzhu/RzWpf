using System;
using System.IO.Packaging;
using System.Windows;

namespace RzWpf
{
    public static class PackUriHelperEx
    {
        public static void RegisterScheme()
        {
            if( !UriParser.IsKnownScheme( "pack" ) )
            {
                UriParser.Register( new GenericUriParser
                    ( GenericUriParserOptions.GenericAuthority ), "pack", -1 );
            }
        }

        public static void Init()
        {
            PackUriHelper.Create( new Uri( "reliable://0" ) );
            new FrameworkElement();
        }
    }
}

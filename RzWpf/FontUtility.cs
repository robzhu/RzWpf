using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace RzWpf
{
    public static class FontUtility
    {
        public static void SetDefaultFont( FontFamily font )
        {
            Debug.Assert( null != font );

            TextElement.FontFamilyProperty.OverrideMetadata( typeof( TextElement ), new FrameworkPropertyMetadata( font ) );
            TextBlock.FontFamilyProperty.OverrideMetadata( typeof( TextBlock ), new FrameworkPropertyMetadata( font ) );
        }

        public static void Test()
        { 
            var families = Fonts.GetFontFamilies( new Uri( "pack://application:,,,/PLUILib;Component/Resources/" ) );
        }
    }
}

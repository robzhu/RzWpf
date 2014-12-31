using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace RzWpf
{
    public sealed class ImageSourceConverter : IValueConverter
    {
        public static BitmapImage CreateBitmapImageFromUri( string absoluteUri )
        {
            if( absoluteUri == null ) return null;

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri( absoluteUri, UriKind.RelativeOrAbsolute );
            img.CacheOption = BitmapCacheOption.Default;
            img.EndInit();
            return img;
        }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( null == value ) return null;
            try
            {
                return CreateBitmapImageFromUri( value as string );
            }
            catch { return null; }
        } 
        
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) 
        {
            throw new NotImplementedException(); 
        }
    }

}

using System.Windows.Media;

namespace RzWpf
{
    public static class ImageSourceExtensions
    {
        public static double CenterX( this ImageSource imageSrc )
        {
            return imageSrc.Width / 2;
        }

        public static double CenterY( this ImageSource imageSrc )
        {
            return imageSrc.Height / 2;
        }
    }
}

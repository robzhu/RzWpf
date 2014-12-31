using System;
using System.Windows;
using System.Windows.Media;
using RzAspects;

namespace RzWpf
{
    public static class SpriteOrigins
    {
        private static Point _topLeft = new Point( 0, 0 );
        public static Point TopLeft
        {
            get { return _topLeft; }
        }

        private static Point _bottomRight = new Point( 1, 1 );
        public static Point BottomRight
        {
            get { return _bottomRight; }
        }

        private static Point _bottomCenter = new Point( 0.5, 1 );
        public static Point BottomCenter
        {
            get { return _bottomCenter; }
        }

        private static Point _center = new Point( 0.5, 0.5 );
        public static Point Center
        {
            get { return _center; }
        }
    }
}

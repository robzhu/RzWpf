using System.Windows;

namespace RzWpf
{
    public static class VectorExtensions
    {
        public static Point ToPoint( this Vector vector )
        {
            return new Point( vector.X, vector.Y );
        }
    }
}

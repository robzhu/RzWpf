using System.Windows;
using System;

namespace RzWpf
{
    public static class VectorUtility
    {
        

        //private Vector UnitVectorWithAngleInZeroToOneScale( double scale )
        //{
        //    double radians = Math.PI * 2 * scale;
        //    return UnitVectorWithAngleRadians( radians );
        //}

        public static Vector UnitVectorWithAngleRadians( double angle )
        {
            return new Vector( Math.Cos( angle ), Math.Sin( angle ) );
        }
    }
}

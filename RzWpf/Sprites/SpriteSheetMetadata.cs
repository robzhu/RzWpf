using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RzWpf
{
    public struct EffectFrame : IComparable
    {
        public int Index;
        public float SourceX;
        public float SourceY;
        public float OriginX;
        public float OriginY;

        private const int NUMFIELDS = 3;

        public static EffectFrame CreateDefault()
        {
            return default( EffectFrame );
        }

        public EffectFrame( string data, float originX, float originY )
        {
            string[] parts = data.Split( new string[] { "," }, System.StringSplitOptions.None );

            Debug.Assert( parts.Length == NUMFIELDS );

            Index = int.Parse( parts[ 0 ] );
            SourceX = float.Parse( parts[ 1 ] );
            SourceY = float.Parse( parts[ 2 ] );
            OriginX = originX;
            OriginY = originY;
        }

        public override string ToString()
        {
            return string.Format( "{0},{1},{2}", Index, SourceX, SourceY );
        }

        public int CompareTo( object obj )
        {
            //Less than zero:       x is less than y.
            //Zero:                 x equals y.
            //Greater than zero:    x is greater than y.

            if( obj == null ) return 1;

            EffectFrame other = (EffectFrame)obj;
            return Index - other.Index;
        }
    }

    public struct SpriteSheetMetadata
    {
        public const string Delimiter = "|";

        public string ModelName { get; set; }
        public string AnimationName { get; set; }

        public float FrameWidth { get; set; }
        public float FrameHeight { get; set; }

        public int NumFrames { get; set; }
        public int NumFrameColumns { get; set; }
        public int NumFrameRows { get; set; }

        public float FrameDuration { get; set; }
        public uint FrameRate { get; set; }

        public float OriginX { get; set; }
        public float OriginY { get; set; }

        public float ProjectileTargetX { get; set; }
        public float ProjectileTargetY { get; set; }

        public List<EffectFrame> EffectFrames { get; set; }

        private const int NUMFIELDS = 14;

        public SpriteSheetMetadata( string serializedData ) : this()
        {
            string[] parts = serializedData.Split( new string[] { Delimiter }, System.StringSplitOptions.None );
            Debug.Assert( parts.Length <= NUMFIELDS );

            ModelName = parts[ 0 ];
            AnimationName = parts[ 1 ];

            FrameWidth = float.Parse( parts[ 2 ] );
            FrameHeight = float.Parse( parts[ 3 ] );

            NumFrames = int.Parse( parts[ 4 ] );
            NumFrameColumns = int.Parse( parts[ 5 ] );
            NumFrameRows = int.Parse( parts[ 6 ] );

            FrameDuration = float.Parse( parts[ 7 ] );
            FrameRate = uint.Parse( parts[ 8 ] );

            OriginX = float.Parse( parts[ 9 ] );
            OriginY = float.Parse( parts[ 10 ] );

            ProjectileTargetX = float.Parse( parts[ 11 ] );
            ProjectileTargetY = float.Parse( parts[ 12 ] );

            EffectFrames = new List<EffectFrame>();

            try
            {
                string effectFramesData = parts[ 13 ];
                effectFramesData = effectFramesData.TrimStart( '<' );
                effectFramesData = effectFramesData.TrimEnd( '>' );

                string[] effectFrameParts = effectFramesData.Split( new string[] { ";" }, System.StringSplitOptions.None );
                foreach( var effectFramePart in effectFrameParts )
                {
                    try
                    {
                        if( !string.IsNullOrEmpty( effectFramePart ) )
                        {
                            EffectFrames.Add( new EffectFrame( effectFramePart, OriginX, OriginY ) );
                        }
                    }
                    catch { }
                }
            }
            catch{ }
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat( "{0}|", ModelName );
            sb.AppendFormat( "{0}|", AnimationName );

            sb.AppendFormat( "{0}|", FrameWidth );
            sb.AppendFormat( "{0}|", FrameHeight );

            sb.AppendFormat( "{0}|", NumFrames );
            sb.AppendFormat( "{0}|", NumFrameColumns );
            sb.AppendFormat( "{0}|", NumFrameRows );

            sb.AppendFormat( "{0}|", FrameDuration );
            sb.AppendFormat( "{0}|", FrameRate );

            sb.AppendFormat( "{0}|", OriginX );
            sb.AppendFormat( "{0}|", OriginY );

            sb.AppendFormat( "{0}|", ProjectileTargetX );
            sb.AppendFormat( "{0}|", ProjectileTargetY );

            //"<frameIndex,SourceX,SourceY; frameIndex,SourceX,SourceY>"
            sb.Append( "<" );

            if( EffectFrames != null )
            {
                for( int i = 0; i < EffectFrames.Count; i++ )
                {
                    var current = EffectFrames[ i ];

                    string format = "{0};";

                    if( i == ( EffectFrames.Count - 1 ) )
                    {
                        //last item
                        format = "{0}";
                    }

                    sb.AppendFormat( format, current.ToString() );
                }
            }

            sb.Append( ">" );

            return sb.ToString();
        }
    }
}

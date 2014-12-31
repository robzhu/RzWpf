using RzAspects;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace RzWpf
{
    public partial class Sprite
    {
        private static Sprite _placeholder = new Sprite( null, 0, 0, new Point( 0, 0 ) );

        public static int AnimationUpdateGroupId { get; set; }

        public static ISprite CreateEmpty()
        {
            return CreateStatic( null );
        }

        public static ISprite CreateStatic( ImageSource image )
        {
            return CreateStatic( image, SpriteOrigins.Center );
        }

        public static ISprite CreateStatic( ImageSource image, Point origin )
        {
            return CreateStatic( image, image == null ? 0 : image.Width, image == null ? 0 : image.Height, origin );
        }

        public static ISprite CreateStatic( ImageSource image, double clipWidth, double clipHeight, Point origin )
        {
            var sprite = new Sprite( image, clipWidth, clipHeight, origin );

            return sprite;
        }

        public static ISprite CreateStaticFromPath( string imagePathUri )
        {
            return CreateStaticFromPath( imagePathUri, SpriteOrigins.BottomCenter );
        }

        public static ISprite CreateStaticFromPath( string imagePathUri, Point origin )
        {
            return CreateStatic( ImageSourceConverter.CreateBitmapImageFromUri( imagePathUri ), origin );
        }

        public static ISprite CreateAnimatedFromPath( string uri )
        {
            var imageSource = ImageSourceConverter.CreateBitmapImageFromUri( uri );
            var metadata = SpriteSheetReader.ReadMetadata( uri );

            metadata.FrameWidth = (float)imageSource.Width / metadata.NumFrameColumns;
            metadata.FrameHeight = (float)imageSource.Height / metadata.NumFrameRows;

            return CreateAnimated( imageSource, metadata );
        }

        public static ISprite CreateAnimated( ImageSource source, SpriteSheetMetadata metadata )
        {
            float frameWidth = metadata.FrameWidth;
            float frameHeight = metadata.FrameHeight;
            Point origin = new Point( metadata.OriginX, metadata.OriginY );
            Point projectileTarget = new Point( metadata.ProjectileTargetX, metadata.ProjectileTargetY );

            var sprite = new Sprite( source, frameWidth, frameHeight, origin, projectileTarget );

            sprite._duration = RzAspects.Duration.CreateWithCustomUpdateGroup( AnimationUpdateGroupId, double.PositiveInfinity, true, metadata.FrameDuration * 1000 );
            sprite._duration.OnPeriodicDurationElapsed += sprite.OnFrame;

            int numFramesX = metadata.NumFrameColumns;
            int numFramesY = metadata.NumFrameRows;

            for( int y = 0; y < numFramesY; y++ )
            {
                for( int x = 0; x < numFramesX; x++ )
                {
                    sprite.FrameClips.Add( new RectangleGeometry( new Rect( x * frameWidth, y * frameHeight, frameWidth, frameHeight ) ) );
                    if( sprite.FrameClips.Count >= metadata.NumFrames ) break;
                }
                if( sprite.FrameClips.Count >= metadata.NumFrames ) break;
            }

            sprite.NumberOfFrames = metadata.NumFrames;
            sprite.LastFrameIndex = metadata.NumFrames - 1;
            if( metadata.EffectFrames.Count > 0 )
            {
                sprite.InitEffectFrames( metadata.EffectFrames );
            }

            sprite.Reset();

            return sprite;
        }
    }
}

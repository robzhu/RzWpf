using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace RzWpf
{
    public interface ISprite : ICanvasElement
    {
        ImageSource Image { get; }
        int Id { get; }
        double ImageWidth { get; }
        double ImageHeight { get; }

        double Width { get; }
        double Height { get; }

        double OffsetX { get; }
        double OffsetY { get; }
        
        float Opacity { get; set; }

        double ScaleX { get; set; }
        double ScaleY { get; set; }

        /// <summary>
        /// The relative point (0-1) on X and Y that the image will be anchored.  
        /// For example, images with origin = (0,0) will be "positioned" by their top left corner.
        /// Images with origin = (1,1) will be positioned by their bottom right corner.
        /// </summary>
        Point Origin { get; }

        Point ProjectileTarget { get; }
        double ProjectileTargetOffsetX { get; }
        double ProjectileTargetOffsetY { get; }

        /// <summary>
        /// Gets or sets the region of the sprite to render.
        /// </summary>
        RectangleGeometry Clip { get; set; }

        double GetClipWidth();
        double GetClipHeight();

        RectangleGeometry TestClip { get; set; }

        /// <summary>
        /// Offsets the Image X coordinate to create the illusion that the clipped region is the full image.
        /// </summary>
        double ImageOffsetX { get; set; }

        /// <summary>
        /// Offsets the Image Y coordinate to create the illusion that the clipped region is the full image.
        /// </summary>
        double ImageOffsetY { get; set; }

        List<EffectFrame> EffectFrames { get; }

        #region Animation properties

        event Action<uint> AnimationCompleted;
        event Action<EffectFrame> OnAnimationEffectFrame;

        int CurrentFrame { get; set; }
        bool Loop { get; set; }
        bool IsEffectFrame { get; }
        bool Paused { get; set; }
        int NumberOfFrames { get; }
        int LastFrameIndex { get; }
        void Reset();

        double Angle { get; set; }

        double CenterX { get; }
        double CenterY { get; }

        bool FlipVertical { get; set; }
        bool FlipHorizontal { get; set; }
        void Rotate( Vector direction );
        void RotateToFace( Point point );
        void RotateToFace( double x, double y );
        #endregion
    }

}

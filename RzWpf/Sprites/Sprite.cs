using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using RzAspects;

namespace RzWpf
{
    public partial class Sprite : ModelBase, ISprite
    {
        public const double DefaultFrameDuration = 200;

        public event Action<uint> AnimationCompleted;
        public event Action<EffectFrame> OnAnimationEffectFrame;

        private List<RectangleGeometry> FrameClips = new List<RectangleGeometry>();
        private List<EffectFrame> _effectFrames = new List<EffectFrame>();
        private List<bool> _effectFrameEventFlags = new List<bool>();
        private int _nextEffectFrameIndex = -1;
        private RzAspects.Duration _duration;
        private uint _loopCount = 0;
        private EffectFrame? _currentEffectFrame;

        #region Properties
        public ImageSource Image { get; private set; }

        public int Id { get; private set; }

        public double ImageWidth
        {
            get { return Image == null ? 0 : Image.Width; }
        }

        public double ImageHeight
        {
            get { return Image == null ? 0 : Image.Height; }
        }

        public double Width { get; private set; }
        public double Height { get; private set; }

        public string PropertyOffsetX { get { return "OffsetX"; } }
        private double _OffsetX = 0;
        public double OffsetX
        {
            get { return _OffsetX; }
            private set { SetProperty( PropertyOffsetX, ref _OffsetX, value ); }
        }

        public string PropertyOffsetY { get { return "OffsetY"; } }
        private double _OffsetY = 0;
        public double OffsetY
        {
            get { return _OffsetY; }
            private set { SetProperty( PropertyOffsetY, ref _OffsetY, value ); }
        }

        public string PropertyCanvasX { get { return "CanvasX"; } }
        private double _CanvasX = 0;
        public double CanvasX
        {
            get { return _CanvasX; }
            set { SetProperty( PropertyCanvasX, ref _CanvasX, value ); }
        }

        public string PropertyCanvasY { get { return "CanvasY"; } }
        private double _CanvasY = 0;
        public double CanvasY
        {
            get { return _CanvasY; }
            set { SetProperty( PropertyCanvasY, ref _CanvasY, value ); }
        }

        public string PropertyZIndex { get { return "ZIndex"; } }
        private int _ZIndex;
        public int ZIndex
        {
            get { return _ZIndex; }
            set { SetProperty( PropertyZIndex, ref _ZIndex, value ); }
        }

        public string PropertyOpacity { get { return "Opacity"; } }
        private float _Opacity = 1;
        public float Opacity
        {
            get { return _Opacity; }
            set { SetProperty( PropertyOpacity, ref _Opacity, value ); }
        }

        public string PropertyScaleX { get { return "ScaleX"; } }
        private double _ScaleX = 1;
        public double ScaleX
        {
            get { return _ScaleX; }
            set { SetProperty( PropertyScaleX, ref _ScaleX, value, RecalculateTransform ); }
        }

        public string PropertyScaleY { get { return "ScaleY"; } }
        private double _ScaleY = 1;
        public double ScaleY
        {
            get { return _ScaleY; }
            set { SetProperty( PropertyScaleY, ref _ScaleY, value, RecalculateTransform ); }
        }

        public string PropertyOrigin { get { return "Origin"; } }
        private Point _Origin;
        public Point Origin
        {
            get { return _Origin; }
            private set { SetProperty( PropertyOrigin, ref _Origin, value ); }
        }

        public Point ProjectileTarget{ get; private set; }
        public double ProjectileTargetOffsetX { get; private set; }
        public double ProjectileTargetOffsetY { get; private set; }

        public string PropertyClip { get { return "Clip"; } }
        private RectangleGeometry _Clip;
        public RectangleGeometry Clip
        {
            get { return _Clip; }
            set { SetProperty( PropertyClip, ref _Clip, value ); }
        }

        public string PropertyTestClip { get { return "TestClip"; } }
        private RectangleGeometry _TestClip;
        public RectangleGeometry TestClip
        {
            get { return _TestClip; }
            set { SetProperty( PropertyTestClip, ref _TestClip, value ); }
        }

        public string PropertyImageOffsetX { get { return "ImageOffsetX"; } }
        private double _ImageOffsetX;
        public double ImageOffsetX
        {
            get { return _ImageOffsetX; }
            set { SetProperty( PropertyImageOffsetX, ref _ImageOffsetX, value ); }
        }

        public string PropertyImageOffsetY { get { return "ImageOffsetY"; } }
        private double _ImageOffsetY;
        public double ImageOffsetY
        {
            get { return _ImageOffsetY; }
            set { SetProperty( PropertyImageOffsetY, ref _ImageOffsetY, value ); }
        }

        public string PropertyVisible { get { return "Visible"; } }
        private bool _Visible = true;
        public bool Visible
        {
            get { return _Visible; }
            set { SetProperty( PropertyVisible, ref _Visible, value ); }
        }

        public string PropertyEffectId { get { return "EffectId"; } }
        private string _EffectId;
        public string EffectId
        {
            get { return _EffectId; }
            set { SetProperty( PropertyEffectId, ref _EffectId, value ); }
        }

        public string PropertyCurrentFrame { get { return "CurrentFrame"; } }
        private int _CurrentFrame = 0;
        public int CurrentFrame
        {
            get { return _CurrentFrame; }
            set { SetProperty( PropertyCurrentFrame, ref _CurrentFrame, value, FrameChanged ); }
        }

        public string PropertyLoop { get { return "Loop"; } }
        private bool _Loop = true;
        public bool Loop
        {
            get { return _Loop; }
            set { SetProperty( PropertyLoop, ref _Loop, value ); }
        }

        public string PropertyIsEffectFrame { get { return "IsEffectFrame"; } }
        private bool _IsEffectFrame = false;
        public bool IsEffectFrame
        {
            get { return _IsEffectFrame; }
            private set { SetProperty( PropertyIsEffectFrame, ref _IsEffectFrame, value ); }
        }

        public string PropertyPaused { get { return "Paused"; } }
        private bool _Paused = false;
        public bool Paused
        {
            get { return _Paused; }
            set { SetProperty( PropertyPaused, ref _Paused, value ); }
        }

        public string PropertyAngle { get { return "Angle"; } }
        private double _Angle = 0;
        public double Angle
        {
            get { return _Angle; }
            set { SetProperty( PropertyAngle, ref _Angle, value ); }
        }

        public string PropertyCenterX { get { return "CenterX"; } }
        private double _CenterX;
        public double CenterX
        {
            get { return _CenterX; }
            private set { SetProperty( PropertyCenterX, ref _CenterX, value ); }
        }

        public string PropertyCenterY { get { return "CenterY"; } }
        private double _CenterY;
        public double CenterY
        {
            get { return _CenterY; }
            private set { SetProperty( PropertyCenterY, ref _CenterY, value ); }
        }

        public string PropertyFlipHorizontal { get { return "FlipHorizontal"; } }
        private bool _FlipHorizontal = false;
        public bool FlipHorizontal
        {
            get { return _FlipHorizontal; }
            set { SetProperty( PropertyFlipHorizontal, ref _FlipHorizontal, value, FlipChanged ); }
        }

        public string PropertyFlipVertical { get { return "FlipVertical"; } }
        private bool _FlipVertical = false;
        public bool FlipVertical
        {
            get { return _FlipVertical; }
            set { SetProperty( PropertyFlipVertical, ref _FlipVertical, value, FlipChanged ); }
        }
        
        public int NumberOfFrames { get; private set; }
        public int LastFrameIndex { get; private set; }

        public List<EffectFrame> EffectFrames 
        {
            get { return _effectFrames; }
        }

        #endregion

        private Sprite( ImageSource image, double clipWidth, double clipHeight, Point origin ) :
            this( image, clipWidth, clipHeight, origin, new Point( 0, 0 ) ) { }

        private static int LastId = 0;

        private Sprite( ImageSource image, double clipWidth, double clipHeight, Point origin, Point projectileTarget )
        {
            if( ( origin.X < 0 ) || ( origin.X > 1.0 ) ) throw new ArgumentException( "origin.X out of bounds" );
            if( ( origin.Y < 0 ) || ( origin.Y > 1.0 ) ) throw new ArgumentException( "origin.Y out of bounds" );

            Width = clipWidth;
            Height = clipHeight;

            Id = LastId++;

            Image = image;
            _Clip = new RectangleGeometry( new Rect( 0, 0, clipWidth, clipHeight ) );
            _Origin = origin;
            _CenterX = _Origin.X * Width;
            _CenterY = _Origin.Y * Height;
            ProjectileTarget = projectileTarget;
            ProjectileTargetOffsetX = ( ( projectileTarget.X - origin.X ) * clipWidth );
            ProjectileTargetOffsetY = ( ( projectileTarget.Y - origin.Y ) * clipHeight );

            RecalculateTransform( clipWidth, clipHeight );
        }

        public double GetClipWidth()
        {
            return Clip.Rect.Width;
        }

        public double GetClipHeight()
        {
            return Clip.Rect.Height;
        }

        private void RecalculateTransform()
        {
            RecalculateTransform( Clip.Rect.Width, Clip.Rect.Height );
        }

        private void RecalculateTransform( double clipWidth, double clipHeight )
        {
            _OffsetX = -clipWidth * Origin.X * ScaleX;
            _OffsetY = -clipHeight * Origin.Y * ScaleY;

            RaisePropertyChanged( PropertyOrigin );
            RaisePropertyChanged( PropertyOffsetX );
            RaisePropertyChanged( PropertyOffsetY );
        }

        public void Reset()
        {
            AnimationCompleted = null;
            OnAnimationEffectFrame = null;
            _duration.Reset();
            _loopCount = 0;
            CurrentFrame = 0;
        }

        private void FrameChanged()
        {
            if( _nextEffectFrameIndex != -1 )
            {
                //The current frame is an effect frame if it is greater than the next effect frame index.
                //This is because the app might drop a frame and skip over the exact effect frame.
                IsEffectFrame = ( CurrentFrame >= _effectFrames[ _nextEffectFrameIndex ].Index ) && !_effectFrameEventFlags[ _nextEffectFrameIndex ];

                if( IsEffectFrame )
                {
                    //Mark this effect frame as having been raised this loop
                    _effectFrameEventFlags[ _nextEffectFrameIndex ] = true;

                    if( ( OnAnimationEffectFrame != null ) )
                    {
                        OnAnimationEffectFrame( _effectFrames[ _nextEffectFrameIndex ] );
                    }

                    if( _nextEffectFrameIndex < ( _effectFrames.Count - 1 ) )
                    {
                        //there are more frames, increment the frame index.
                        _nextEffectFrameIndex++;
                    }
                    else
                    {
                        //we have reached the end of the frame count, wrap around and go to frame 0.
                        _nextEffectFrameIndex = 0;
                    }
                }
            }

            Clip = FrameClips[ CurrentFrame ];

            if( ( CurrentFrame == 0 ) && ( _loopCount > 0 ) )
            {
                //RaiseCompleted();
                //This effect frame was the last effect frame, reset the effect frame event flags
                for( int i = 0; i < _effectFrameEventFlags.Count; i++ )
                {
                    _effectFrameEventFlags[ i ] = false;
                }
            }
        }

        private void OnAnimationLoopCompleted()
        {
            _loopCount++;
            if( AnimationCompleted != null )
            {
                AnimationCompleted( _loopCount );
            }

            //reset the effect frame event flags
            for( int i = 0; i < _effectFrameEventFlags.Count; i++ )
            {
                _effectFrameEventFlags[ i ] = false;
            }
        }

        private void OnFrame( int period )
        {
            if( Paused ) return;
            if( ( CurrentFrame == LastFrameIndex ) && !Loop ) return;

            int nextFrameIndex = CurrentFrame + 1;
            if( nextFrameIndex > LastFrameIndex )
            {
                nextFrameIndex = 0;

                if( _effectFrames.Count > 0 )
                {
                    _nextEffectFrameIndex = 0;
                }
            }

            CurrentFrame = nextFrameIndex;
            if( CurrentFrame == LastFrameIndex )
            {
                OnAnimationLoopCompleted();
            }
        }

        private void FlipChanged()
        {
            if( ( ( ScaleX > 0 ) && FlipHorizontal ) || 
                ( ( ScaleX < 0 ) && !FlipHorizontal ) )
            {
                ScaleX = -ScaleX;
            }

            if( ( ( ScaleY > 0 ) && FlipVertical ) || 
                ( ( ScaleY < 0 ) && !FlipVertical ) )
            {
                ScaleY = -ScaleY;
            }
        }

        private Vector _unitX = new Vector( 1, 0 );
        public void Rotate( Vector direction )
        {
            Angle = Vector.AngleBetween( direction, _unitX );
        }

        public void RotateToFace( Point point )
        {
            RotateToFace( point.X, point.Y );
        }

        public void RotateToFace( double x, double y )
        {
            //CanvasY - y is not a bug, this is because of the windows coordinate system ( y increases from top to bottom ).
            Vector facing = new Vector( x - CanvasX, CanvasY - y );
            Rotate( facing );
        }

        private void InitEffectFrames( List<EffectFrame> effectFrames )
        {
            _effectFrames.AddRange( effectFrames );

            for( int i = 0; i < effectFrames.Count; i++ )
            {
                //Initialize the collection that tracks whether an effect frame event has been raised.
                _effectFrameEventFlags.Add( false );
            }
            _nextEffectFrameIndex = 0;
        }
    }
}

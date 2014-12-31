using RzAspects;

namespace RzWpf
{
    public class SpriteContainer : ModelBase, ICanvasElement
    {
        public string PropertyCurrent { get { return "Current"; } }
        private ISprite _Current;
        public ISprite Current
        {
            get { return _Current; }
            set { SetProperty( PropertyCurrent, ref _Current, value ); }
        }

        public string PropertyZIndex { get { return "ZIndex"; } }
        private int _ZIndex;
        public int ZIndex
        {
            get { return _ZIndex; }
            set { SetProperty( PropertyZIndex, ref _ZIndex, value ); }
        }

        public string PropertyCanvasX { get { return "CanvasX"; } }
        private double _CanvasX;
        public double CanvasX
        {
            get { return _CanvasX; }
            set { SetProperty( PropertyCanvasX, ref _CanvasX, value ); }
        }

        public string PropertyCanvasY { get { return "CanvasY"; } }
        private double _CanvasY;
        public double CanvasY
        {
            get { return _CanvasY; }
            set { SetProperty( PropertyCanvasY, ref _CanvasY, value ); }
        }

        public string PropertyEffectId { get { return "EffectId"; } }
        private string _EffectId;
        public string EffectId
        {
            get { return _EffectId; }
            set { SetProperty( PropertyEffectId, ref _EffectId, value ); }
        }

        public string PropertyVisible { get { return "Visible"; } }
        private bool _Visible = true;
        public bool Visible
        {
            get { return _Visible; }
            set { SetProperty( PropertyVisible, ref _Visible, value ); }
        }

        protected override void AfterPropertyChange<T>( string propertyName, ref T property, T newValue )
        {
            Refresh();
        }

        private void Refresh()
        {
            if( Current != null )
            {
                Current.EffectId = EffectId;
                Current.ZIndex = ZIndex;
                Current.CanvasX = CanvasX;
                Current.CanvasY = CanvasY;
                Current.Visible = Visible;
            }
        }
    }
}

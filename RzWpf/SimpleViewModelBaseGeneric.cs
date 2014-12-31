using RzAspects;
using System.ComponentModel;

namespace RzWpf
{
    public abstract class SimpleViewModelBase : CompositePropertyChangeNotificationBase
    {

    }

    public abstract class SimpleViewModelBase<ModelType> : CompositePropertyChangeNotificationBase
    {
        public string Property_Model { get { return "Model"; } }

        private ModelType _model;
        public ModelType Model 
        {
            get { return _model; }
            set
            {
                bool modelChanged = false;

                if( ( null == _model ) && ( null == value ) )
                    return;
                else if( ( null == _model ) && ( null != value ) )
                {
                    OnBeforeModelChanged();
                    SetProperty( Property_Model, ref _model, value );
                    AttachModelChangeHandler( _model as INotifyPropertyChanged );
                    modelChanged = true;
                }
                else if( ( null != _model ) && ( null == value ) )
                {
                    OnBeforeModelChanged();
                    DetachModelChangeHandler( _model as INotifyPropertyChanged );
                    SetProperty( Property_Model, ref _model, value );
                    modelChanged = true;
                }
                else //( ( null != _model ) && ( null != value ) )
                {
                    if( _model.Equals( value ) ) return;

                    DetachModelChangeHandler( _model as INotifyPropertyChanged );
                    OnBeforeModelChanged();
                    SetProperty( Property_Model, ref _model, value );
                    AttachModelChangeHandler( _model as INotifyPropertyChanged );
                    modelChanged = true;
                }

                if( modelChanged )
                {
                    OnModelChanged();
                    RaiseAllPropertyChangedNotification();
                }
            } 
        }

        protected virtual void OnBeforeModelChanged() { }
        protected virtual void OnModelChanged() { }

        private void DetachModelChangeHandler( INotifyPropertyChanged model )
        {
            if( null == model ) return;
            model.PropertyChanged -= new PropertyChangedEventHandler( OnModelPropertyChanged );
        }

        private void AttachModelChangeHandler( INotifyPropertyChanged model )
        {
            if( null == model ) return;
            model.PropertyChanged += new PropertyChangedEventHandler( OnModelPropertyChanged );
        }

        protected virtual void OnModelPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            RaisePropertyChanged( e.PropertyName );
        }
    }
}

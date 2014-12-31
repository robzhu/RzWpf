using RzWpf.Validation;

namespace RzWpf
{
    /// <summary>
    /// This class is responsible for provided base functionality to all ViewModels such as setting properties, 
    /// raising property change events, and template methods for the view model processing pipeline.  
    /// </summary>
    public abstract class ViewModelBase : ValidationElement, IViewModel
    {
    }
}

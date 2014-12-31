using RzWpf.Validation;

namespace RzWpf
{
    public interface IViewModel : IValidationElement
    {
    }

    public interface IViewModel<ModelType> : IViewModel
    {
        ModelType Model { get; set; }
    }
}

using System.ComponentModel;

namespace RzWpf.Validation
{
    /// <summary>
    /// Interface for a logical element whose content can be either valid or invalid.
    /// </summary>
    public interface IValidationElement : IDataErrorInfo
    {
        /// <summary>
        /// Checks to see if the content of this element is in a valid state.
        /// </summary>
        /// <returns>True if the content is valid.  False otherwise.</returns>
        bool IsContentValid();
    }
}
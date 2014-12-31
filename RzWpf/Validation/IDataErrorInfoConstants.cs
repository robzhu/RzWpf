
namespace RzWpf.Validation
{
    /// <summary>
    /// This class contains constants relating to the use of IDataErrorInfo
    /// </summary>
    public static class IDataErrorInfoConstants
    {
        /// <summary>
        /// When IDataErrorInfo validates a property, it uses string.empty to indicate success.  
        /// Any other string represents the validation error.
        /// </summary>
        public static string ValidationResultSuccess { get { return string.Empty; } }
    }
}
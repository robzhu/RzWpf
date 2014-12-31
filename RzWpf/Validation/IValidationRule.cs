
namespace RzWpf.Validation
{
    /// <summary>
    /// Interface for a validation rule, which checks for a particular condition.
    /// </summary>
    public interface IValidationRule
    {
        /// <summary>
        /// Validates the rule with the specified parameter.
        /// </summary>
        /// <param name="param">The parameter for the rule.</param>
        /// <param name="context">The context for the parameter.</context>
        /// <returns>True if the validation rule passed.  False otherwise.</returns>
        bool IsValid( object param, object context );
    }
}
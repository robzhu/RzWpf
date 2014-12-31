using RzWpf.Localization;

namespace RzWpf.Validation
{
    /// <summary>
    /// Interface for a validation attribute, which expresses a validity rule for the property it decorates.
    /// </summary>
    public interface IPropertyValidationAttribute : IResourceString, IValidationRule
    {
        /// <summary>
        /// The condition under which this validation rule should be run.  Condition must map to a predicate property.  
        /// </summary>
        string Condition { get; set; }

        /// <summary>
        /// Retrieves the error message when this validation rule fails.
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Determines whether the predicate condition for this validation rule is met by the specified instance.
        /// </summary>
        /// <param name="instance">The object to validate.</param>
        /// <returns>True if the object's properties meets the specified condition (and this rule should be run), false otherwise.</returns>
        bool IsConditionMet( object instance );
    }
}
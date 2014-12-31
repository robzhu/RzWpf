using RzAspects;

namespace RzWpf.Validation
{
    /// <summary>
    /// This attribute is used to decorate ViewModel properties that should not be validated as part of the owner's composition.  
    /// </summary>
    public sealed class ExcludeChildValidationElementAttribute : PropertyValidationAttribute
    {
        public ExcludeChildValidationElementAttribute()
            : base( new TypeChecker<IValidationElement>() ){ }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            return true;
        }
    }
}
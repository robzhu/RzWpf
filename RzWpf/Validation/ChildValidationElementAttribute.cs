using RzAspects;

namespace RzWpf.Validation
{
    /// <summary>
    /// This attribute is used to decorate properties that implement IValidationElement.  The validation rule will 
    /// delegate validation to the property's IsContentValid() method.  
    /// </summary>
    /// <remarks>A null value is considered valid because the element is effectively undefined.  To add null checking, 
    /// add a PropertyNullable attribute.</remarks>
    public class ChildValidationElementAttribute : PropertyValidationAttribute
    {
        public ChildValidationElementAttribute()
            : base( new TypeChecker<IValidationElement>() ){ }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            IValidationElement validationElement = toValidate as IValidationElement;
            if( null != validationElement )
            {
                return validationElement.IsContentValid();
            }
            return true;
        }
    }
}
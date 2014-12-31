using RzAspects;

namespace RzWpf.Validation
{
    /// <summary>
    /// Validation attribute that indicates decorated property must equal a specified boolean variable.
    /// </summary>
    public sealed class BooleanMustEqualAttribute : PropertyValidationAttribute
    {
        public bool Value { get; set; }

        public BooleanMustEqualAttribute() : base( new TypeChecker<bool>() ) { }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            bool value = (bool)toValidate;
            return ( value == Value );
        }
    }
}
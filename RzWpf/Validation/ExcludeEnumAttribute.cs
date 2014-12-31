using RzAspects;

namespace RzWpf.Validation
{
    /// <summary>
    /// Validation attribute that requires the decorated property to NOT equal a specified enum value.
    /// </summary>
    public sealed class ExcludeEnumAttribute : PropertyValidationAttribute
    {
        public int Exclude { get; set; }

        public ExcludeEnumAttribute() : base( new TypeChecker<int>() ){ }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            return Exclude != (int)toValidate;
        }
    }
}
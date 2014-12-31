using RzAspects;

namespace RzWpf.Validation
{
    ///<summary>
    ///Validation attribute that requires the decorated property to NOT equal a specified string value.
    ///</summary>
    public sealed class ExcludeStringAttribute : PropertyValidationAttribute
    {
        public string Exclude { get; set; }

        public ExcludeStringAttribute() : base( new TypeChecker<string>() ) { }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            return Exclude != (string)toValidate;
        }
    }
}
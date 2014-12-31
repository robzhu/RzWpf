using System.Globalization;
using RzAspects;

namespace RzWpf.Validation
{
    ///<summary>
    ///Validation attribute that requires the decorated string to NOT end in a specified string value.
    ///</summary>
    public sealed class StringMustNotEndWithAttribute : PropertyValidationAttribute
    {
        public StringMustNotEndWithAttribute() : base( new TypeChecker<string>() ) { }

        public string CannotEndIn { get; set; }
        public bool IgnoreCase { get; set; }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            if( null == toValidate )
            {
                return true;
            }

            string strToValidate = (string)toValidate;
            return !strToValidate.EndsWith( CannotEndIn, IgnoreCase, CultureInfo.CurrentCulture );
        }
    }
}
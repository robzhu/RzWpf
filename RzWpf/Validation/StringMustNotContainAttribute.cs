using System.Globalization;
using RzAspects;

namespace RzWpf.Validation
{
    ///<summary>
    ///Validation attribute that requires the decorated string to NOT contain a specified set of characters.
    ///</summary>
    public sealed class StringMustNotContainAttribute : PropertyValidationAttribute
    {
        public StringMustNotContainAttribute() : base( new TypeChecker<string>() ) { }

        public string ExcludedChars { get; set; }
        public bool IgnoreCase { get; set; }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            if( null == toValidate )
            {
                return true;
            }

            string strToValidate = (string)toValidate;
            string excludedChars = ExcludedChars;

            if( IgnoreCase )
            {
                strToValidate = strToValidate.ToLower( CultureInfo.CurrentCulture );
                excludedChars = excludedChars.ToLower( CultureInfo.CurrentCulture );
            }

            return ( -1 == strToValidate.IndexOfAny( excludedChars.ToCharArray() ) );
        }
    }
}
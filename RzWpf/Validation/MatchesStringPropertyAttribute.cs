using System.Reflection;
using RzAspects;

namespace RzWpf.Validation
{
    ///<summary>
    ///Validation attribute that requires the decorated string property to equal the value of the specified string property 
    ///</summary>
    public sealed class MatchesStringPropertyAttribute : PropertyValidationAttribute
    {
        public string MatchProperty { get; set; }

        public MatchesStringPropertyAttribute() : base( new TypeChecker<string>() ) { }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            if( ( null == toValidate ) || ( null == context ) )
            {
                return false;
            }

            string strToValidate = toValidate as string;

            if( null == strToValidate )
            {
                return false;
            }

            PropertyInfo propInfo = context.GetType().GetProperty( MatchProperty, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
            if( null == propInfo )
            {
                return false;
            }

            string strToMatch = propInfo.GetValue( context, null ) as string;

            if( null == strToMatch )
            {
                return false;
            }

            return strToValidate == strToMatch;
        }
    }
}
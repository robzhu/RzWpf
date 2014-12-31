using System.Text.RegularExpressions;
using RzAspects;

namespace RzWpf.Validation
{
    ///<summary>
    ///Validation attribute that requires the decorated property to match the specified regular expression
    ///</summary>
    public sealed class MatchesRegexAttribute : PropertyValidationAttribute
    {
        Regex _expression;
        string _expressionString;

        public string Expression 
        {
            get { return _expressionString; }
            set
            {
                if( _expressionString != value )
                {
                    _expressionString = value;
                    _expression = new Regex( _expressionString );
                }
            }
        }

        public MatchesRegexAttribute() : base( new TypeChecker<string>() ) { }

        protected override bool ValidateInternal( object toValidate, object context )
        {
            if( null != _expression )
            {
                string strToValidate = ( null != toValidate ) ? (string)toValidate : string.Empty;
                return _expression.IsMatch( strToValidate );
            }
            return true;
        }
    }
}
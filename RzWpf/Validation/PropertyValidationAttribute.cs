using System;
using System.Reflection;
using RzAspects;
using RzWpf.Localization;

namespace RzWpf.Validation
{
    /// <summary>
    /// Base class for validation attributes. 
    /// </summary>
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = true )]
    public abstract class PropertyValidationAttribute : ResourceStringAttribute, IPropertyValidationAttribute
    {
        public string Condition { get; set; }

        public string ErrorMessage
        {
            get
            {
                return ResourceValue;
            }
        }

        ITypeChecker ValidationType { get; set; }

        public PropertyValidationAttribute() : this( new TypeChecker<object>() ) { }

        protected PropertyValidationAttribute( ITypeChecker validationType )
        {
            if( null == validationType )
            {
                throw new ArgumentNullException( "validationType" );
            }
            ValidationType = validationType;
        }

        /// <summary>
        /// Checks to see if a property condition is true on this instance.
        /// </summary>
        /// <param name="condition">
        /// The name of the condition property.  If the property does not exist or does not return 
        /// a boolean type, the condition is assumed to be met.  
        /// </param>
        /// <returns>True if the condition doesn't exist/does not return null/is met.  False if the condition is not met.</returns>
        public bool IsConditionMet( object instance )
        {
            if( string.IsNullOrEmpty( Condition ) )
            {
                //No condition specified, so it is met by default.
                return true;
            }

            PropertyInfo propInfo = instance.GetType().GetProperty( Condition, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
            if( null == propInfo )
            {
                //The condition could not be resolved
                return true;
            }
            else
            {
                object value = propInfo.GetValue( instance, null );
                if( value is bool )
                {
                    return (bool)value;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsValid( object toValidate )
        {
            return IsValid( toValidate, null );
        }

        public bool IsValid( object toValidate, object context )
        {
            if( ( null != context ) && ( !IsConditionMet( context ) ) )
            {
                //This attribute's condition is not met; return true to indicate that it will not block validation
                return true;
            }

            if( ValidationType.IsOfMatchingType( toValidate ) )
            {
                return ValidateInternal( toValidate, context );
            }
            else
            {
                //the parameter to validate against did not match the type, no possible way it can satisfy the validation rule.
                return false;
            }
        }

        protected abstract bool ValidateInternal( object toValidate, object context );
    }
}
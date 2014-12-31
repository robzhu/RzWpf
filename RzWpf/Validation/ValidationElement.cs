using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using RzAspects;

namespace RzWpf.Validation
{
    /// <summary>
    /// Base class for objects that have properties and validation rules on those properties.
    /// </summary>
    public abstract class ValidationElement : CompositePropertyChangeNotificationBase, IValidationElement
    {
        private Dictionary<PropertyInfo, List<IPropertyValidationAttribute>> _validationEntries =
            new Dictionary<PropertyInfo, List<IPropertyValidationAttribute>>();

        protected ValidationElement()
        {
            InitializeValidationEntries();
        }

        #region Properties

        /// <summary>
        /// Leverage this property to disable validation when your control is disabled or hidden. 
        /// When a control is either disabled or hidden, user input in not applicable and therefore validation 
        /// should always succeeds.
        /// </summary>
        private bool _isValidationEnabled = true;
        public bool IsValidationEnabled
        {
            get { return _isValidationEnabled; }
            set { _isValidationEnabled = value; }
        }

        public string Error { get; protected set; }

        public string this[ string propertyName ]
        {
            get { return ValidateProperty( propertyName ); }
        }

        protected Dictionary<PropertyInfo, List<IPropertyValidationAttribute>> ValidationEntries
        {
            get { return _validationEntries; }
        }
        #endregion

        private void AddAttributeRulesForProperty( PropertyInfo propInfo )
        {
            foreach( object obj in propInfo.GetCustomAttributes( typeof( IPropertyValidationAttribute ), true ) )
            {
                var validationAttribute = obj as IPropertyValidationAttribute;
                if( null != validationAttribute )
                {
                    validationAttribute.ValidateResourceDescription();

                    AddPropertyValidationRule( propInfo, validationAttribute );
                }
            }
        }

        private void AddChildValidationElementRule( PropertyInfo propInfo )
        {
            bool foundExplicitValidationAttribute = false;
            foreach( object obj in propInfo.GetCustomAttributes( typeof( IPropertyValidationAttribute ), true ) )
            {
                IPropertyValidationAttribute validationAttribute = obj as ChildValidationElementAttribute;
                if( null != validationAttribute )
                {
                    AddPropertyValidationRule( propInfo, validationAttribute );
                    foundExplicitValidationAttribute = true;
                    break;
                }
                
                //Maybe the declaration wants to exclude the element from being validated
                validationAttribute = obj as ExcludeChildValidationElementAttribute;
                if( null != validationAttribute )
                {
                    foundExplicitValidationAttribute = true;
                    break;
                }
            }

            if( !foundExplicitValidationAttribute )
            {
                AddPropertyValidationRule( propInfo, new ChildValidationElementAttribute() );
            }
        }

        private void AddPropertyValidationRule( PropertyInfo propInfo, IPropertyValidationAttribute rule )
        {
            if( _validationEntries.ContainsKey( propInfo ) )
            {
                List<IPropertyValidationAttribute> rules = _validationEntries[ propInfo ];
                rules.Add( rule );
            }
            else
            {
                List<IPropertyValidationAttribute> rules = new List<IPropertyValidationAttribute>();
                rules.Add( rule );
                _validationEntries.Add( propInfo, rules );
            }
        }

        void InitializeValidationEntries()
        {
            foreach( PropertyInfo propInfo in GetType().GetProperties() )
            {
                if( TypeUtility.PropertyImplementsInterface( propInfo, typeof( IValidationElement ) ) )
                {
                    //This property is a nested Validation Element.  Defer the validation rule. 
                    AddChildValidationElementRule( propInfo );
                }
                else
                {
                    AddAttributeRulesForProperty( propInfo );
                }
            }
        }

        public virtual bool IsContentValid()
        {
            if (!this.IsValidationEnabled)
            {
                // when the content is not active, user input should not be validated.
                return true;
            }

            //null validation result string means the property is valid.
            string validationResult = null;

            foreach( PropertyInfo propInfo in _validationEntries.Keys )
            {
                validationResult = ValidateProperty( propInfo );
                if( !string.IsNullOrEmpty( validationResult ) )
                {
                    return false;
                }
            }
            return true;
        }

        private string ValidateProperty( PropertyInfo propInfo )
        {
            if( null == propInfo )
            {
                throw new ArgumentException( "propInfo cannot be null", "propInfo" );
            }

            string result = IDataErrorInfoConstants.ValidationResultSuccess;

            if( !_validationEntries.ContainsKey( propInfo ) )
            {
                //This property does not have any corresponding validation entires, assume that it is valid.
                return result;
            }

            List<IPropertyValidationAttribute> rulesForProperty = _validationEntries[ propInfo ];

            foreach( IPropertyValidationAttribute rule in rulesForProperty )
            {
                //Note: if the rule's condition is not met, it will not run (which indicates that the decorated property is valid).
                if( !rule.IsValid( propInfo.GetValue( this, null ), this ) )
                {
                    //TODO, robzhu: adding child validation element rules automatically without associated resource key.
                    result = rule.ErrorMessage;
                    break;
                }
            }

            return result;
        }

        private string ValidateProperty( string propertyName )
        {
            return ValidateProperty( GetType().GetProperty( propertyName ) );
        }

        /// <summary>
        /// Raises property changed events for all validation-decorated properties.  This will cause all validation
        /// rules to be rerun.  
        /// </summary>
        /// <remarks>
        /// Use this method judiciously if data binding via UpdatedSourceTrigger=OnPropertyChanged and you have many validation rules/properties.
        /// </remarks>
        protected void ReValidate()
        {
            foreach (var propInfo in ValidationEntries.Keys)
            {
                RaisePropertyChanged( propInfo.Name );
            }
        }

        protected bool SetProperty<T>(string propertyName, ref T property, T newValue, bool reValidate = false)
        {
            bool propertyChanged = base.SetProperty( propertyName, ref property, newValue );

            if (propertyChanged)
            {
                if (reValidate)
                {
                    ReValidate();
                }
                return true;
            }

            return propertyChanged;
        }
    }
}
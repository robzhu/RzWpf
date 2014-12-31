using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenInheritingValidationElement
    {
        public delegate bool IsContentValidDelegate();
        public delegate bool ValidateCallbackDelegate(object obj);

        class NameValidationAttribute : PropertyValidationAttribute
        {
            public static ValidateCallbackDelegate ValidateCallback { get; set; }

            protected override bool ValidateInternal(object toValidate, object context)
            {
                return ValidateCallback( toValidate );
            }
        }

        class NameValidation2Attribute : PropertyValidationAttribute
        {
            public static ValidateCallbackDelegate ValidateCallback { get; set; }

            protected override bool ValidateInternal(object toValidate, object context)
            {
                return ValidateCallback( toValidate );
            }
        }

        class AgeValidationAttribute : PropertyValidationAttribute
        {
            public static ValidateCallbackDelegate ValidateCallback { get; set; }

            protected override bool ValidateInternal(object toValidate, object context)
            {
                return ValidateCallback( toValidate );
            }
        }

        class CustomChildValidationElementAttribute : ChildValidationElementAttribute
        {
            public static ValidateCallbackDelegate ValidateCallback { get; set; }

            protected override bool ValidateInternal(object toValidate, object context)
            {
                return ValidateCallback( toValidate );
            }
        }

        class SimpleValidationElement : ValidationElement
        {
            [NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public string Name { get; set; }

            [AgeValidationAttribute( ResourceKey = "Error_Age_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public int Age { get; set; }
        }

        class ValidationElementWithNoValidationAttributes : ValidationElement
        {
            public string Name { get; set; }
        }

        class IncorrectlyDecoratedElement : ValidationElement
        {
            //Validation attributes must define resource key and resource manager
            [NameValidationAttribute]
            public string Name { get; set; }
        }

        class PartiallyDecoratedElement : ValidationElement
        {
            [NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public string Name { get; set; }
            public object UnvalidatedProperty { get; set; }
        }

        class DoubleDecoratedElement : ValidationElement
        {
            [NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            [NameValidation2Attribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public string Name { get; set; }
        }


        class ParentValidationElement : ValidationElement
        {
            /// <summary>
            /// Note that ChildValidationElement is ommitted here.  During instantiation, the ValidationElement base class
            /// will still add a ChildValidationElement attribute to this property because it is a ValidationElement.
            /// To override this behavior, add an attribute that derives from ChildValidationElementAttribute that does nothing. 
            /// </summary>
            public ValidationElement Child { get; set; }
        }

        class ChildValidationElement : ValidationElement
        {
            IsContentValidDelegate _isContentValidCallback = null;
            public ChildValidationElement(IsContentValidDelegate validateCallback)
            {
                _isContentValidCallback = validateCallback;
            }

            public override bool IsContentValid()
            {
                return _isContentValidCallback();
            }
        }

        class ParentWithExplicitChildValidationElement : ValidationElement
        {
            /// <summary>
            /// An explicitly decorated child validation element should override the default behavior of ChildValidationElement
            /// </summary>
            [CustomChildValidationElement( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public ValidationElement Child { get; set; }
        }

        class UnrelatedAttribute : PropertyValidationAttribute
        {
            protected override bool ValidateInternal(object toValidate, object context)
            {
                return true;
            }
        }

        class ParentWithMixedChildValidationElement : ValidationElement
        {
            [CustomChildValidationElement( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public ValidationElement Child { get; set; }

            [UnrelatedAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public ValidationElement Child2 { get; set; }
        }

        class ConditionalValidationElement : ValidationElement
        {
            /// <summary>
            /// Adding a condition definition to the validation attribute causes to it to evaluate the validation rule iff the condition predicate is met.
            /// In this class, ValidateName must be true for the NameValidationAttribute to be evaluated.
            /// </summary>
            [NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ), Condition = "ValidateName" )]
            public string Name { get; set; }

            public bool ValidateName { get; set; }
        }

        class BadConditionalValidationElement : ValidationElement
        {
            /// <summary>
            /// Condition of "Meow" refers to a predicate that does not exist.  Despite this, the validation rule should still be run. 
            /// </summary>
            [NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ), Condition = "Meow" )]
            public string Name { get; set; }
        }

        class BadConditionalValidationElement2 : ValidationElement
        {
            /// <summary>
            /// Condition of "Meow" refers to a string instead of a predicate.  Despite this, the validation rule should still run. 
            /// </summary>
            [NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ), Condition = "Meow" )]
            public string Name { get; set; }

            public string Meow
            {
                get { return "meow"; }
            }
        }

        class MockViewModelWithForcedRevalidate : ValidationElement
        {
            private int _trigger;
            public int Trigger
            {
                get { return _trigger; }
                set
                {
                    //NOTE: the trigger property specifies true for the final parameter (reValidate)
                    //The default for this parameter is false.  By setting true, this property indicates
                    //that all other properties on this ViewModel that have validation attributes should be
                    //re-validated.  It does this by raising OnPropertyChanged with the names of all the 
                    //other decorated properties.  
                    SetProperty( "Trigger", ref _trigger, value, true );
                }
            }

            private int _undecoratedValue;
            public int UndecoratedValue
            {
                get { return _undecoratedValue; }
                set
                {
                    SetProperty( "UndecoratedValue", ref _undecoratedValue, value );
                }
            }

            private int _decoratedValue;

            //The specific validation attribute doesn't matter here.  The fact that this property is decorated
            //means the base view model will detect it and refresh it when force re-validate occurs.
            [ExcludeEnum( Exclude = 5, ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public int DecoratedValue
            {
                get { return _decoratedValue; }
                set
                {
                    SetProperty( "DecoratedValue", ref _decoratedValue, value );
                }
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            NameValidationAttribute.ValidateCallback = null;
            NameValidation2Attribute.ValidateCallback = null;
            AgeValidationAttribute.ValidateCallback = null;
            CustomChildValidationElementAttribute.ValidateCallback = null;
        }

        [TestMethod]
        public void ConstructingIncorrectlyDecoratedElementThrows()
        {
            try
            {
                var ve = new IncorrectlyDecoratedElement();
                Assert.Fail( "should not reach here" );
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void ValidationElementWithNoValidationAttributesWorks()
        {
            var ve = new ValidationElementWithNoValidationAttributes();
        }

        [TestMethod]
        public void PropertyIndexerCallsValidationAttribute()
        {
            bool ValidationAttributeCalled = false;
            NameValidationAttribute.ValidateCallback = (obj) =>
            {
                ValidationAttributeCalled = true;
                return false;
            };

            var ve = new SimpleValidationElement();

            Assert.IsFalse( ValidationAttributeCalled );

            string result = ve["Name"];

            Assert.IsTrue( ValidationAttributeCalled );
        }

        [TestMethod]
        public void IsContentValidCallsAllValidationRules()
        {
            bool NameValidationAttributeCalled = false;
            bool AgeValidationAttributeCalled = false;
            NameValidationAttribute.ValidateCallback = (obj) =>
            {
                NameValidationAttributeCalled = true;
                return true;
            };

            AgeValidationAttribute.ValidateCallback = (obj) =>
            {
                AgeValidationAttributeCalled = true;
                return true;
            };

            var ve = new SimpleValidationElement();

            Assert.IsFalse( NameValidationAttributeCalled );
            Assert.IsFalse( AgeValidationAttributeCalled );

            ve.IsContentValid();

            Assert.IsTrue( NameValidationAttributeCalled );
            Assert.IsTrue( AgeValidationAttributeCalled );
        }

        [TestMethod]
        public void PartiallyDecoratedElementAlwaysReturnsValidForUndecoratedProperty()
        {
            var ve = new PartiallyDecoratedElement();
            ve.UnvalidatedProperty = null;
            Assert.AreEqual( IDataErrorInfoConstants.ValidationResultSuccess, ve["UnvalidatedProperty"] );
            ve.UnvalidatedProperty = new object();
            Assert.AreEqual( IDataErrorInfoConstants.ValidationResultSuccess, ve["UnvalidatedProperty"] );
        }

        [TestMethod]
        public void PartiallyDecoratedElementIgnoresUndecoratedProperties()
        {
            bool ValidationResult = false;
            NameValidationAttribute.ValidateCallback = (obj) =>
            {
                return ValidationResult;
            };

            var ve = new PartiallyDecoratedElement();

            Assert.AreEqual( ValidationResult, ve.IsContentValid() );
            ValidationResult = true;
            Assert.AreEqual( ValidationResult, ve.IsContentValid() );

            ve.UnvalidatedProperty = new object();

            Assert.AreEqual( ValidationResult, ve.IsContentValid() );

            ve.UnvalidatedProperty = null;

            Assert.AreEqual( ValidationResult, ve.IsContentValid() );
        }

        [TestMethod]
        public void DoubleDecoratedElementValidatesBothAttributes()
        {
            bool NameValidationAttributeCalled = false;
            bool NameValidation2AttributeCalled = false;
            NameValidationAttribute.ValidateCallback = (obj) =>
            {
                NameValidationAttributeCalled = true;
                return true;
            };

            NameValidation2Attribute.ValidateCallback = (obj) =>
            {
                NameValidation2AttributeCalled = true;
                return true;
            };

            var ve = new DoubleDecoratedElement();

            Assert.IsFalse( NameValidationAttributeCalled );
            Assert.IsFalse( NameValidation2AttributeCalled );

            ve.IsContentValid();

            Assert.IsTrue( NameValidationAttributeCalled );
            Assert.IsTrue( NameValidation2AttributeCalled );
        }

        [TestMethod]
        public void ValidationIsNotPerformedWhenIsValidationEnabledIsFalse()
        {
            NameValidationAttribute.ValidateCallback = (obj) =>
            {
                return false;
            };

            var ve = new PartiallyDecoratedElement();
            Assert.AreEqual( false, ve.IsContentValid() );

            ve.IsValidationEnabled = false;
            Assert.AreEqual( true, ve.IsContentValid() );
        }

        [TestMethod]
        public void ValidatingParentElementCallsIsContentValidOnChildElement()
        {
            bool IsContentValidCalled = false;

            var pe = new ParentValidationElement();
            pe.Child = new ChildValidationElement( () =>
            {
                IsContentValidCalled = true;
                return true;
            } );

            Assert.AreEqual( false, IsContentValidCalled );
            pe.IsContentValid();
            Assert.AreEqual( true, IsContentValidCalled );
        }

        [TestMethod]
        public void ValidatingParentWithExplicitChildElementCallsIsCustomValidator()
        {
            bool IsContentValidCalled = false;
            bool CustomValidateCalled = false;

            var pe = new ParentWithExplicitChildValidationElement();

            CustomChildValidationElementAttribute.ValidateCallback = (obj) =>
            {
                CustomValidateCalled = true;
                return true;
            };

            pe.Child = new ChildValidationElement( () =>
            {
                IsContentValidCalled = true;
                return true;
            } );

            Assert.AreEqual( false, IsContentValidCalled );
            Assert.AreEqual( false, CustomValidateCalled );

            pe.IsContentValid();

            Assert.AreEqual( false, IsContentValidCalled );
            Assert.AreEqual( true, CustomValidateCalled );
        }

        [TestMethod]
        public void ChildWithCustomValidationAttributeWorks()
        {
            var pe = new ParentWithMixedChildValidationElement();
        }

        [TestMethod]
        public void ConditionalAttributeIsOnlyCalledIfConditionIsTrue()
        {
            bool ValidationResult = false;
            bool ValidationCalled = false;
            NameValidationAttribute.ValidateCallback = (obj) =>
            {
                ValidationCalled = true;
                return ValidationResult;
            };

            var ve = new ConditionalValidationElement();

            Assert.AreEqual( false, ValidationCalled );

            ve.ValidateName = false;
            ve.IsContentValid();

            Assert.AreEqual( false, ValidationCalled );

            ve.ValidateName = true;
            ve.IsContentValid();

            Assert.AreEqual( true, ValidationCalled );
        }

        [TestMethod]
        public void ValidationRuleStillRunsIfConditionNotFound()
        {
            bool ValidationResult = false;
            bool ValidationCalled = false;
            NameValidationAttribute.ValidateCallback = (obj) =>
            {
                ValidationCalled = true;
                return ValidationResult;
            };

            var ve = new BadConditionalValidationElement();

            Assert.AreEqual( false, ValidationCalled );

            ve.IsContentValid();

            Assert.AreEqual( true, ValidationCalled );
        }

        [TestMethod]
        public void ValidationRuleStillRunsIfConditionIsNotPredicate()
        {
            bool ValidationResult = false;
            bool ValidationCalled = false;
            NameValidationAttribute.ValidateCallback = (obj) =>
            {
                ValidationCalled = true;
                return ValidationResult;
            };

            var ve = new BadConditionalValidationElement2();

            Assert.AreEqual( false, ValidationCalled );

            ve.IsContentValid();

            Assert.AreEqual( true, ValidationCalled );
        }

        [TestMethod]
        public void GetErrorStringForNonExistentPropertyThrowsException()
        {
            var ve = new SimpleValidationElement();

            try
            {
                string error = ve["Meow"];
                Assert.Fail( "should not reach here" );
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void CallingSetPropertyWithRevalidateFlagRaisesPropertyChangedForDecoratedProperties()
        {
            var mock = new MockViewModelWithForcedRevalidate();
            bool decoratedPropertyChanged = false;
            bool undecoratedPropertyChanged = false;
            mock.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "UndecoratedValue")
                    undecoratedPropertyChanged = true;

                if (args.PropertyName == "DecoratedValue")
                    decoratedPropertyChanged = true;
            };

            Assert.IsFalse( decoratedPropertyChanged );
            Assert.IsFalse( undecoratedPropertyChanged );

            mock.Trigger = mock.Trigger;

            Assert.IsFalse( decoratedPropertyChanged );
            Assert.IsFalse( undecoratedPropertyChanged );

            mock.Trigger += 1;

            Assert.IsTrue( decoratedPropertyChanged );
            Assert.IsFalse( undecoratedPropertyChanged );
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzAspects;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenInheritingPropertyValidationAttribute
    {
        class MockValidationAttribute : PropertyValidationAttribute
        {
            public bool _ValidateInternalCalled = false;
            public object _ValidateInternalParameter = null;
            public bool _ValidationIsSuccessful = false;

            protected override bool ValidateInternal( object toValidate, object context )
            {
                _ValidateInternalCalled = true;
                _ValidateInternalParameter = toValidate;
                return _ValidationIsSuccessful;
            }
        }

        class MockValidationAttributeWithNullType : PropertyValidationAttribute
        {
            public MockValidationAttributeWithNullType()
                : base( null )
            {
            }

            protected override bool ValidateInternal( object toValidate, object context )
            {
                return false;
            }
        }

        class MockObjectValidationAttribute : PropertyValidationAttribute
        {
            public delegate bool ValidationCallback( object param );

            private ValidationCallback _callback;
            public MockObjectValidationAttribute( ValidationCallback callback )
                : base() 
            {
                _callback = callback;
            }

            protected override bool ValidateInternal( object toValidate, object context )
            {
                return _callback.Invoke( toValidate );
            }
        }

        class MockIntValidationAttribute : PropertyValidationAttribute
        {
            public delegate bool ValidationCallback( int param );

            private ValidationCallback _callback;
            public MockIntValidationAttribute( ValidationCallback callback )
                : base( new TypeChecker<int>() )
            {
                _callback = callback;
            }

            protected override bool ValidateInternal( object toValidate, object context )
            {
                return _callback.Invoke( (int)toValidate );
            }
        }

        class MockBooleanValidationAttribute : PropertyValidationAttribute
        {
            public MockBooleanValidationAttribute()
                : base( new TypeChecker<bool>() )
            {
            }

            protected override bool ValidateInternal( object toValidate, object context )
            {
                return false;
            }
        }

        class NameValidationAttribute : PropertyValidationAttribute
        {
            public delegate bool ValidateCallbackDelegate( object obj );
            public ValidateCallbackDelegate ValidateCallback { get; set; }

            public NameValidationAttribute( ValidateCallbackDelegate callback )
            {
                ValidateCallback = callback;
            }

            protected override bool ValidateInternal( object toValidate, object context )
            {
                return ValidateCallback( toValidate );
            }
        }

        [TestMethod]
        public void ValidateCallsValidateInternalWithExpectedObject()
        {
            var mock = new MockValidationAttribute();
            Assert.AreEqual( false, mock._ValidateInternalCalled );
            Assert.AreEqual( null, mock._ValidateInternalParameter );

            object parameter = new object();
            mock.IsValid( parameter );
            Assert.AreEqual( true, mock._ValidateInternalCalled );
            Assert.AreEqual( parameter, mock._ValidateInternalParameter );
        }

        [TestMethod]
        public void DefaultReturnMessageWhenValidationFailsIsNotEmpty()
        {
            var mock = new MockValidationAttribute();
            mock._ValidationIsSuccessful = false;

            Assert.IsFalse( mock.IsValid( null ) );
        }

        [TestMethod]
        public void SuccessfulValidationReturnMessageIsEmpty()
        {
            var mock = new MockValidationAttribute();
            mock._ValidationIsSuccessful = true;

            Assert.AreEqual( true, mock.IsValid( null ) );
        }

        [TestMethod]
        public void DerivedTypeWithNullTypeCheckerThrowsExceptionDuringConstruction()
        {
            try
            {
                var mock = new MockValidationAttributeWithNullType();
                Assert.Fail( "should not reach here" );
            }
            catch( ArgumentNullException )
            {
            }
        }

        [TestMethod]
        public void DerivedValidationAttributeCallingParameterlessBaseConstructorAcceptsObjectType()
        {
            bool validateInternalCalled = false;
            var mock = new MockObjectValidationAttribute( ( obj ) =>
            {
                validateInternalCalled = true;
                return false;
            } );

            Assert.IsFalse( validateInternalCalled );

            mock.IsValid( new object() );

            Assert.IsTrue( validateInternalCalled );
        }

        [TestMethod]
        public void CallingValidateWithMismatchedTypesDoesNotInvokeValidateInternal()
        {
            bool validateInternalCalled = false;
            var mock = new MockIntValidationAttribute( ( obj ) =>
            {
                validateInternalCalled = true;
                return false;
            } );

            Assert.IsFalse( validateInternalCalled );

            mock.IsValid( new object() );

            Assert.IsFalse( validateInternalCalled );
        }

        class MockConditionalSubject
        {
            public bool RunValidation { get; set; }

            public string NotAPredicate { get; set; }

            //[NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) )]
            public string Name_Unconditional { get; set; }

            //[NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ), Condition = "RunValidation" )]
            public string Name_Conditional { get; set; }

            //[NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ), Condition = "Meow" )]
            public string Name_InvalidCondition { get; set; }

            //[NameValidationAttribute( ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ), Condition = "NotAPredicate" )]
            public string Name_NonPredicateCondition { get; set; }
        }

        [TestMethod]
        public void UnconditionalValidationAttributeAlwaysRuns()
        {
            bool ValidateCalled = false;
            bool ValidationResult = false;

            var attribute = new NameValidationAttribute( ( obj ) =>
            {
                ValidateCalled = true;
                return ValidationResult;
            } );

            Assert.IsFalse( ValidateCalled );
            var subject = new MockConditionalSubject();

            attribute.IsValid( "meow", subject );

            Assert.IsTrue( ValidateCalled );
        }

        [TestMethod]
        public void ConditionalValidationAttributeRunsWhenConditionIsMet()
        {
            bool ValidateCalled = false;
            bool ValidationResult = false;
            var attribute = new NameValidationAttribute( ( obj ) =>
            {
                ValidateCalled = true;
                return ValidationResult;
            } );
            attribute.Condition = "RunValidation";

            Assert.IsFalse( ValidateCalled );

            var subject = new MockConditionalSubject();
            subject.RunValidation = true;

            attribute.IsValid( "meow", subject );

            Assert.IsTrue( ValidateCalled );
        }

        [TestMethod]
        public void ConditionalValidationAttributeNotRunWhenConditionIsNotMet()
        {
            bool ValidateCalled = false;
            bool ValidationResult = false;
            var attribute = new NameValidationAttribute( ( obj ) =>
            {
                ValidateCalled = true;
                return ValidationResult;
            } );
            attribute.Condition = "RunValidation";

            Assert.IsFalse( ValidateCalled );

            var subject = new MockConditionalSubject();
            subject.RunValidation = false;

            attribute.IsValid( "meow", subject );

            Assert.IsFalse( ValidateCalled );
        }

        [TestMethod]
        public void ValidationAttributeRunsWhenConditionDoesNotExist()
        {
            bool ValidateCalled = false;
            bool ValidationResult = false;
            var attribute = new NameValidationAttribute( ( obj ) =>
            {
                ValidateCalled = true;
                return ValidationResult;
            } );
            attribute.Condition = "Meow";

            Assert.IsFalse( ValidateCalled );

            var subject = new MockConditionalSubject();

            attribute.IsValid( "meow", subject );

            Assert.IsTrue( ValidateCalled );
        }

        [TestMethod]
        public void ValidationAttributeRunsWhenConditionIsNotPredicate()
        {
            bool ValidateCalled = false;
            bool ValidationResult = false;
            var attribute = new NameValidationAttribute( ( obj ) =>
            {
                ValidateCalled = true;
                return ValidationResult;
            } );
            attribute.Condition = "NotAPredicate";

            Assert.IsFalse( ValidateCalled );

            var subject = new MockConditionalSubject();

            attribute.IsValid( "meow", subject );

            Assert.IsTrue( ValidateCalled );
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingExcludeChildValidationElementAttribute
    {
        class ShouldBeCalledAttribute : PropertyValidationAttribute
        {
            public delegate bool ValidateDelegate( object toValidate, object context );
            public static ValidateDelegate ValidateCallback { get; set; }

            protected override bool ValidateInternal( object toValidate, object context )
            {
                return ValidateCallback( toValidate, context );
            }
        }

        class ShouldNotBeCalledAttribute : PropertyValidationAttribute
        {
            public delegate bool ValidateDelegate( object toValidate, object context );
            public static ValidateDelegate ValidateCallback { get; set; }

            protected override bool ValidateInternal( object toValidate, object context )
            {
                return ValidateCallback( toValidate, context );
            }
        }

        class FavouriteChildValidationElement : ValidationElement
        {
            [ShouldBeCalledAttribute( ResourceKey = "TestResource", ResourceManagerSource = typeof( TestResources ) )]
            public string Name { get; set; }
        }

        class NeglectedChildValidationElement : ValidationElement
        {
            [ShouldNotBeCalledAttribute( ResourceKey = "TestResource", ResourceManagerSource = typeof( TestResources ) )]
            public string Name { get; set; }
        }

        class MockValidationElement : ValidationElement
        {
            public FavouriteChildValidationElement FavouriteChild { get; set; }

            [ExcludeChildValidationElement( ResourceKey = "TestResource", ResourceManagerSource = typeof( TestResources ) )]
            public NeglectedChildValidationElement NeglectedChild { get; set; }
        }

        [TestMethod]
        public void ExcludeChildValidationElementAttributeCausesPropertyToBeExcludedFromValidation()
        {
            bool shouldValidateCallBackCalled = false;
            ShouldBeCalledAttribute.ValidateCallback = ( obj, context ) =>
            {
                shouldValidateCallBackCalled = true;
                return true;
            };

            bool shouldNotValidateCallBackCalled = false;
            ShouldNotBeCalledAttribute.ValidateCallback = ( obj, context ) =>
            {
                shouldNotValidateCallBackCalled = true;
                return true;
            };

            var ve = new MockValidationElement();
            ve.FavouriteChild = new FavouriteChildValidationElement();
            ve.NeglectedChild = new NeglectedChildValidationElement();

            Assert.IsFalse( shouldValidateCallBackCalled );
            Assert.IsFalse( shouldNotValidateCallBackCalled );
            ve.IsContentValid();
            Assert.IsTrue( shouldValidateCallBackCalled );
            Assert.IsFalse( shouldNotValidateCallBackCalled );
        }
    }
}
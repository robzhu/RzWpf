using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingCustomValidationAttribute
    {
        class MockValidationElement : IValidationElement
        {
            public delegate bool IsContentValidDelegate();
            private IsContentValidDelegate _callback;

            public MockValidationElement( IsContentValidDelegate callback )
            {
                _callback = callback;
            }

            public bool IsContentValid()
            {
                return _callback.Invoke();
            }

            public string Error { get; set; }

            public string this[ string columnName ]
            {
                get { throw new System.NotImplementedException(); }
            }
        }

        [TestMethod]
        public void IsContentValidGetsCalledOnValidationParameter()
        {
            bool callBackCalled = false;
            var ve = new MockValidationElement( () => 
            { 
                callBackCalled = true;
                return false;
            } );

            var va = new ChildValidationElementAttribute();
            
            Assert.AreEqual( false, callBackCalled );
            va.IsValid( ve );
            Assert.AreEqual( true, callBackCalled );
        }

        [TestMethod]
        public void ValidationResultIsDelegatedToValidationElement()
        {
            bool isContentValid = false;
            var ve = new MockValidationElement( () =>
            {
                return isContentValid;
            } );

            var va = new ChildValidationElementAttribute();

            isContentValid = false;
            Assert.AreEqual( isContentValid, va.IsValid( ve ) );
            isContentValid = true;
            Assert.AreEqual( isContentValid, va.IsValid( ve ) );
        }

        [TestMethod]
        public void NullValidationParameterIsConsideredValid()
        {
            var va = new ChildValidationElementAttribute();
            Assert.AreEqual( true, va.IsValid( null ) );
        }
    }
}

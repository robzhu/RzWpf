using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingBooleanMustEqualAttribute
    {
        [TestMethod]
        public void SimpleValidCaseYieldsIsValidTrue()
        {
            var attribute = new BooleanMustEqualAttribute();
            attribute.Value = true;

            Assert.AreEqual( true, attribute.IsValid( true ) );
        }

        [TestMethod]
        public void SimpleInvalidCaseYieldsIsValidFalse()
        {
            var attribute = new BooleanMustEqualAttribute();
            attribute.Value = true;

            Assert.AreEqual( false, attribute.IsValid( false ) );
        }
    }
}

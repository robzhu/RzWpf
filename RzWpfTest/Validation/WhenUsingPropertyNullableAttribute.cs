using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingPropertyNullableAttribute
    {
        [TestMethod]
        public void NullParameterYieldsIsInvalid()
        {
            var va = new PropertyNullableAttribute();
            va.IsNullable = false;
            Assert.AreEqual( false, va.IsValid( null ) );
        }

        [TestMethod]
        public void NonNullParameterIsValid()
        {
            var va = new PropertyNullableAttribute();
            va.IsNullable = false;
            Assert.AreEqual( true, va.IsValid( new object() ) );
        }
    }
}

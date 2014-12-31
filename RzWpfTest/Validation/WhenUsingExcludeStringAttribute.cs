using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingExcludeStringAttribute
    {
        [TestMethod]
        public void ValidatingExcludedValueYieldsFalse()
        {
            var exclude = new ExcludeStringAttribute();
            exclude.Exclude = "meow";

            Assert.AreEqual( false, exclude.IsValid( "meow" ) );
        }

        [TestMethod]
        public void ValidatingDifferentValueYieldsTrue()
        {
            var exclude = new ExcludeStringAttribute();
            exclude.Exclude = "meow";

            Assert.AreEqual( true, exclude.IsValid( "woof" ) );
        }

        [TestMethod]
        public void ValidatingNonStringTypeReturnsInvalid()
        {
            var exclude = new ExcludeStringAttribute();
            exclude.Exclude = "meow";

            Assert.AreEqual( false, exclude.IsValid( new object() ) );
        }
    }
}

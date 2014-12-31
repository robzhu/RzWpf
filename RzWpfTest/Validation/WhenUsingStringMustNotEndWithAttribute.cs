using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingStringDoesNotEndWithAttribute
    {
        [TestMethod]
        public void DefaultInstanceIgnoresCase()
        {
            var attribute = new StringMustNotEndWithAttribute();
            Assert.IsFalse( attribute.IgnoreCase );
        }

        [TestMethod]
        public void InvalidValueYieldsFalse()
        {
            var attribute = new StringMustNotEndWithAttribute();
            attribute.CannotEndIn = "meow";

            Assert.AreEqual( false, attribute.IsValid( "meow" ) );
        }

        [TestMethod]
        public void InvalidValueWithMismatchedCaseYieldsTrue()
        {
            var attribute = new StringMustNotEndWithAttribute();
            attribute.CannotEndIn = "meow";

            Assert.AreEqual( true, attribute.IsValid( "MEOW" ) );
            Assert.AreEqual( true, attribute.IsValid( "MeoW" ) );
            Assert.AreEqual( true, attribute.IsValid( "MEow" ) );
        }

        [TestMethod]
        public void InvalidValueWithMismatchedCaseAndIgnoreCaseYieldsFalse()
        {
            var attribute = new StringMustNotEndWithAttribute();
            attribute.CannotEndIn = "meow";
            attribute.IgnoreCase = true;

            Assert.AreEqual( false, attribute.IsValid( "MEOW" ) );
            Assert.AreEqual( false, attribute.IsValid( "MeoW" ) );
            Assert.AreEqual( false, attribute.IsValid( "MEow" ) );
        }

        [TestMethod]
        public void ValidValueYieldsTrue()
        {
            var attribute = new StringMustNotEndWithAttribute();
            attribute.CannotEndIn = "meow";

            Assert.AreEqual( true, attribute.IsValid( "meow1" ) );
        }

        [TestMethod]
        public void ValidateNullYieldsTrue()
        {
            var attribute = new StringMustNotEndWithAttribute();
            Assert.AreEqual( true, attribute.IsValid( null ) );
        }
    }
}

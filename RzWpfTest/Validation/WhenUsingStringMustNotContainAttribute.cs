using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingStringMustNotContainAttribute
    {
        [TestMethod]
        public void IgnoresCaseIsFalseByDefault()
        {
            var attribute = new StringMustNotContainAttribute();
            Assert.IsFalse( attribute.IgnoreCase );
        }

        [TestMethod]
        public void InvalidValueYieldsFalse()
        {
            var attribute = new StringMustNotContainAttribute();
            attribute.ExcludedChars = "abc";

            Assert.AreEqual( false, attribute.IsValid( "abc" ) );
        }

        [TestMethod]
        public void InvalidValueForCaseSensitiveYieldsFalse()
        {
            var attribute = new StringMustNotContainAttribute();
            attribute.ExcludedChars = "abc";

            Assert.AreEqual( false, attribute.IsValid( "abc" ) );
        }

        [TestMethod]
        public void ValidValueYieldsTrue()
        {
            var attribute = new StringMustNotContainAttribute();
            attribute.ExcludedChars = "def";

            Assert.AreEqual( true, attribute.IsValid( "abc" ) );
        }

        [TestMethod]
        public void CaseMismatchValueYieldsTrue()
        {
            var attribute = new StringMustNotContainAttribute();
            attribute.ExcludedChars = "abc";

            Assert.AreEqual( true, attribute.IsValid( "ABC" ) );
        }

        [TestMethod]
        public void CaseMismatchValueWithIgnoreCaseYieldsFalse()
        {
            var attribute = new StringMustNotContainAttribute();
            attribute.ExcludedChars = "abc";
            attribute.IgnoreCase = true;

            Assert.AreEqual( false, attribute.IsValid( "ABC" ) );
        }

        [TestMethod]
        public void ValidateNullYieldsTrue()
        {
            var attribute = new StringMustNotContainAttribute();

            Assert.AreEqual( true, attribute.IsValid( null ) );
        }
    }
}

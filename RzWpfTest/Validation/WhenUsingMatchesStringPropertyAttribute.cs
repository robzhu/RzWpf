using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingMatchesStringPropertyAttribute
    {
        class MockValidationElement : ViewModelBase
        {
            public string ToMatch { get; set; }
            public string ToTest { get; set; }
        }

        [TestMethod]
        public void NullContextFailsValidation()
        {
            var attribute = new MatchesStringPropertyAttribute() { MatchProperty = "ToMatch" };
            Assert.IsFalse( attribute.IsValid( "meow", null ) );
        }

        [TestMethod]
        public void NonexistentMatchPropertyReturnsFalse()
        {
            var attribute = new MatchesStringPropertyAttribute() { MatchProperty = "PropertyThatDoesNotExist" };
            var subject = new MockValidationElement();
            Assert.IsFalse( attribute.IsValid( "meow", subject ) );
        }

        [TestMethod]
        public void MatchingPropertiesReturnsObjectIsValid()
        {
            var attribute = new MatchesStringPropertyAttribute() { MatchProperty = "ToMatch" };
            var subject = new MockValidationElement();
            string valueToMatch = "meow";
            subject.ToMatch = valueToMatch;

            Assert.IsTrue( attribute.IsValid( valueToMatch , subject ) );
        }

        [TestMethod]
        public void NonMatchingPropertiesReturnsObjectIsInvalid()
        {
            var attribute = new MatchesStringPropertyAttribute() { MatchProperty = "ToMatch" };
            var subject = new MockValidationElement();
            string valueToMatch = "meow";
            subject.ToMatch = valueToMatch;

            Assert.IsFalse( attribute.IsValid( "woof" , subject ) );
        }
    }
}

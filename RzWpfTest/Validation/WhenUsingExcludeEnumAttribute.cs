using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingExcludeEnumAttribute
    {
        enum MockEnum
        {
            Meow,
            Woof,
        }

        [TestMethod]
        public void ValidatingExcludedEnumYieldsIsValidFalse()
        {
            var exclude = new ExcludeEnumAttribute();
            exclude.Exclude = (int)MockEnum.Meow;

            Assert.AreEqual( false, exclude.IsValid( MockEnum.Meow ) );
        }

        [TestMethod]
        public void ValidatingNonExcludedEnumYieldsIsValidTrue()
        {
            var exclude = new ExcludeEnumAttribute();
            exclude.Exclude = (int)MockEnum.Meow;

            Assert.AreEqual( true, exclude.IsValid( MockEnum.Woof ) );
        }
    }
}

using System.Globalization;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingEnumEqualityToVisibilityConverter
    {
        enum MyEnum
        {
            Meow,
            Woof,
            Moo
        }

        [TestMethod]
        public void DefaultEqualityIsVisible()
        {
            var converter = new EnumEqualityToVisibilityConverter();

            var result = converter.Convert( MyEnum.Meow, typeof( MyEnum ), "Meow", CultureInfo.InvariantCulture );
            Assert.IsTrue( result.Equals( Visibility.Visible ) );
        }

        [TestMethod]
        public void NullValueReturnsUnsetValue()
        {
            var converter = new EnumEqualityToVisibilityConverter();

            var result = converter.Convert( null, typeof( MyEnum ), "Meow", CultureInfo.InvariantCulture );
            Assert.IsTrue( result == DependencyProperty.UnsetValue );
        }

        [TestMethod]
        public void NullParameterReturnsNotEqualValue()
        {
            var converter = new EnumEqualityToVisibilityConverter();

            var result = converter.Convert( MyEnum.Moo, typeof( MyEnum ), null, CultureInfo.InvariantCulture );
            Assert.IsTrue( result == DependencyProperty.UnsetValue );
        }

        [TestMethod]
        public void MismatchedParameterTypeReturnsUnsetValue()
        {
            var converter = new EnumEqualityToVisibilityConverter();

            var result = converter.Convert( MyEnum.Moo, typeof( MyEnum ), 1, CultureInfo.InvariantCulture );
            Assert.IsTrue( result == DependencyProperty.UnsetValue );
        }

        [TestMethod]
        public void IncompatibleParameterTypeReturnsUnsetValue()
        {
            var converter = new EnumEqualityToVisibilityConverter();

            var result = converter.Convert( MyEnum.Moo, typeof( MyEnum ), "bar", CultureInfo.InvariantCulture );
            Assert.IsTrue( result == DependencyProperty.UnsetValue );
        }

        [TestMethod]
        public void ConvertBackReturnsUnsetValue()
        {
            var converter = new EnumEqualityToVisibilityConverter();
            var result = converter.ConvertBack( MyEnum.Moo, typeof( MyEnum ), null, CultureInfo.InvariantCulture );
            Assert.IsTrue( result == DependencyProperty.UnsetValue );
        }
    }
}

using System.Globalization;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingEnumToBoolConverter
    {
        enum TestEnum
        {
            Foo, 
            Bar
        }

        [TestMethod]
        public void ConvertForEqualsCaseWorks()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.Convert( TestEnum.Foo, null, "Foo", CultureInfo.CurrentCulture );
            Assert.AreEqual( true, result );
        }

        [TestMethod]
        public void ConvertForDifferentCaseWorks()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.Convert( TestEnum.Bar, null, "Foo", CultureInfo.CurrentCulture );
            Assert.AreEqual( false, result );
        }

        [TestMethod]
        public void ConvertForUnmappedCaseReturnsUnset()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.Convert( TestEnum.Bar, null, "Meow", CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }

        [TestMethod]
        public void ConvertNullParameterStringReturnsUnset()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.Convert( TestEnum.Bar, null, null, CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }

        [TestMethod]
        public void ConvertForNonEnumTypeReturnsUnset()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.Convert( "meow", null, "Foo", CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }

        [TestMethod]
        public void ConvertBackWorks()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.ConvertBack( null, typeof( TestEnum ), "Foo", CultureInfo.CurrentCulture );
            Assert.AreEqual( TestEnum.Foo, result );
        }

        [TestMethod]
        public void ConvertBackWithUnmappedReturnsUnset()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.ConvertBack( null, typeof( TestEnum ), "Meow", CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }

        [TestMethod]
        public void ConvertBackForNonStringParameterReturnsUnset()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.ConvertBack( null, typeof( TestEnum ), new object(), CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }

        [TestMethod]
        public void ConvertBackForNullParameterReturnsUnset()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.ConvertBack( null, typeof( TestEnum ), null, CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }

        [TestMethod]
        public void ConvertBackForEmptyStringReturnsUnset()
        {
            var conv = new EnumToBoolConverter();

            object result = conv.ConvertBack( null, typeof( TestEnum ), string.Empty, CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }
    }
}

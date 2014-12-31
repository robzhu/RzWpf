using System.Globalization;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingBoolToVisibilityConverter
    {
        [TestMethod]
        public void TrueConvertsToVisibleEnum()
        {
            var converter = new BoolToVisibilityConverter();
            object result = converter.Convert( true, typeof( Visibility ), null, CultureInfo.CurrentCulture );
            Assert.AreEqual( Visibility.Visible, result );
        }

        [TestMethod]
        public void FalseConvertsToCollapsedEnum()
        {
            var converter = new BoolToVisibilityConverter();
            object result = converter.Convert( false, typeof( Visibility ), null, CultureInfo.CurrentCulture );
            Assert.AreEqual( Visibility.Collapsed, result );
        }

        [TestMethod]
        public void VisibleEnumConvertsBackToTrue()
        {
            var converter = new BoolToVisibilityConverter();
            object result = converter.ConvertBack( Visibility.Visible, typeof( bool ), null, CultureInfo.CurrentCulture );
            Assert.AreEqual( true, result );
        }

        [TestMethod]
        public void CollapsedEnumConvertsBackToFalse()
        {
            var converter = new BoolToVisibilityConverter();
            object result = converter.ConvertBack( Visibility.Collapsed, typeof( bool ), null, CultureInfo.CurrentCulture );
            Assert.AreEqual( false, result );
        }

        [TestMethod]
        public void NullConvertsToUnsetValue()
        {
            var converter = new BoolToVisibilityConverter();
            object result = converter.Convert( null, typeof( Visibility ), null, CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }

        [TestMethod]
        public void NullConvertsBackToUnsetValue()
        {
            var converter = new BoolToVisibilityConverter();
            object result = converter.ConvertBack( null, typeof( bool ), null, CultureInfo.CurrentCulture );
            Assert.AreEqual( DependencyProperty.UnsetValue, result );
        }
    }
}

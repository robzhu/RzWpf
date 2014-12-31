using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingMatchesRegexAttribute
    {
        [TestMethod]
        public void AlphaNumericRegexRuleWorks()
        {
            //other common regexes can be found at: http://regexlib.com/Search.aspx

            var va = new MatchesRegexAttribute();

            va.Expression = "^[a-zA-Z0-9]+$";
            
            Assert.AreEqual( true, va.IsValid( "abc" ) );
            Assert.AreEqual( true, va.IsValid( "123" ) );
            Assert.AreEqual( true, va.IsValid( "abc123" ) );
            Assert.AreEqual( false, va.IsValid( "#abc" ) );
            Assert.AreEqual( false, va.IsValid( "12!3" ) );
            Assert.AreEqual( false, va.IsValid( "ab*c123" ) );
            Assert.AreEqual( false, va.IsValid( "&" ) );
        }
    }
}

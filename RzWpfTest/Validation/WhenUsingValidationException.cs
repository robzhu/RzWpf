using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingValidationException
    {
        [TestMethod]
        public void DefaultConstructorMessageIsNotNullOrEmpty()
        {
            var ve = new ValidationException();
            Assert.IsFalse( string.IsNullOrEmpty( ve.Message ) );
        }

        [TestMethod]
        public void NewValidationExceptionWithStringParamHasStringMessage()
        {
            string message = "meow";
            var ve = new ValidationException( message );
            Assert.AreEqual( message, ve.Message );
        }

        [TestMethod]
        public void NewValidationExceptionWithParameterizedStringWorks()
        {
            string messageFormat = "meow, {0}";
            string messageParam = "meow";
            string finalMessage = string.Format( messageFormat, messageParam );
            var ve = new ValidationException( messageFormat, messageParam );
            Assert.AreEqual( finalMessage, ve.Message );
        }

        [TestMethod]
        public void NewValidationExceptionWithInnerExceptionWorks()
        {
            Exception innerException = new Exception();
            var ve = new ValidationException( "meow", innerException );
            Assert.AreEqual( innerException, ve.InnerException );
        }

        [TestMethod]
        public void NewValidationExceptionWithInnerExceptionAndFormatStringWorks()
        {
            string messageFormat = "meow, {0}";
            string messageParam = "meow";
            string finalMessage = string.Format( messageFormat, messageParam );
            Exception innerException = new Exception();
            var ve = new ValidationException( innerException, messageFormat, messageParam );
            Assert.AreEqual( finalMessage, ve.Message );
            Assert.AreEqual( innerException, ve.InnerException );
        }
    }
}

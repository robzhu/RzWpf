using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf;
using System.Windows.Input;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingCommandManagerHelper
    {
        [TestMethod]
        public void InvalidateRequerySuggestedAndProcessWorks()
        {
            bool requerySuggestedCalled = false;
            CommandManager.RequerySuggested += ( sender, args ) =>
                {
                    requerySuggestedCalled = true;
                };

            Assert.IsFalse( requerySuggestedCalled );

            CommandManagerHelper.InvalidateRequerySuggestedAndProcess();

            Assert.IsTrue( requerySuggestedCalled );
        }
    }
    
}


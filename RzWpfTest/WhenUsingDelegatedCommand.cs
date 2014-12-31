using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RzWpfTest
{
    [TestClass]
    public class WhenUsingDelegatedCommand
    {
        [TestMethod]
        public void ExecuteCallsActionDelegate()
        {
            bool delegateCalled = false;
            DelegatedCommand command = new DelegatedCommand( () =>
            {
                delegateCalled = true;
            } );

            command.Execute( new object() );

            Assert.AreEqual( true, delegateCalled );
        }

        [TestMethod]
        public void NewDelegatedCommandWithNullActionThrowsException()
        {
            try
            {
                var command = new DelegatedCommand( null, () => { return true; } );
                Assert.Fail( "should not reach here" );
            }
            catch( ArgumentNullException )
            {
            }
        }

        [TestMethod]
        public void CanExecuteCallsCanExecuteDelegate()
        {
            object parameter = new object();
            object calledWith = null;
            DelegatedCommand<object> command = new DelegatedCommand<object>( ( obj ) => { },
                ( obj ) =>
                {
                    calledWith = obj;
                    return true;
                }
            );

            command.CanExecute( parameter );

            Assert.AreEqual( parameter, calledWith );
        }

        [TestMethod]
        public void NullCanExecuteReturnsTrueForCanExecute()
        {
            DelegatedCommand command = new DelegatedCommand( () => { }, null );

            Assert.AreEqual( true, command.CanExecute( null ) );
        }

        [TestMethod]
        public void CanExecuteWithLexicalClosureWorks()
        {
            bool canExecute = false;
            DelegatedCommand command = new DelegatedCommand( () => { },
                () =>
                {
                    return canExecute;
                }
            );

            Assert.AreEqual( false, command.CanExecute( null ) );

            canExecute = true;
            Assert.AreEqual( true, command.CanExecute( null ) );
        }

        [TestMethod]
        public void AddAndRemoveHandlerToCanExecuteWorks()
        {
            var command = new DelegatedCommand( () => { } );

            EventHandler callback = ( sender, e ) => { };

            command.CanExecuteChanged += callback;
            command.CanExecuteChanged -= callback;
        }

        [TestMethod]
        public void CanExecuteCallbackRegistersItselfWithCommandManager()
        {
            var command = new DelegatedCommand( () => { }, () => { return true; } );
            bool canExecuteCallbackCalled = false;
            EventHandler callback = ( sender, e ) =>
            {
                canExecuteCallbackCalled = true;
            };

            command.CanExecuteChanged += callback;

            Assert.IsFalse( canExecuteCallbackCalled );

            CommandManagerHelper.InvalidateRequerySuggestedAndProcess();

            Assert.IsTrue( canExecuteCallbackCalled );

            canExecuteCallbackCalled = false;
            command.CanExecuteChanged -= callback;

            CommandManagerHelper.InvalidateRequerySuggestedAndProcess();

            Assert.IsFalse( canExecuteCallbackCalled );
        }

        [TestMethod]
        public void WhenExecutedAsyncReturnsAwaitableTaskForExecution()
        {
            var command = DelegatedCommand.CreateAwaitableCommand();
            var task = command.WhenExecutedAsync();

            Task.Run( () =>
            {
                Thread.Sleep( 1000 );
                Assert.Fail( "test took too long" );
            } );

            Task.Run( () =>
            {
                Thread.Sleep( 50 );
                command.Execute( null );
            } );

            task.Wait();
        }
    }
}

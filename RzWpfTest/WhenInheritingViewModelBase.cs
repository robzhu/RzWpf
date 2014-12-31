using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf;
using RzWpf.Validation;

namespace RzWpfTest
{
    [TestClass]
    public class WhenInheritingViewModelBase
    {
        class MockChildViewModel : ViewModelBase
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set { SetProperty( "Name", ref _name, value ); }
            }
        }

        class MockParentViewModel : ViewModelBase
        {
            private string _name;
            public string Name
            {
                get { return _name; }
                set { SetProperty( "Name", ref _name, value ); }
            }

            private MockChildViewModel _child = new MockChildViewModel();
            public MockChildViewModel Child
            {
                get { return _child; }
                set { SetProperty( "Child", ref _child, value ); }
            }
        }

        class MockGrandParentViewModel : ViewModelBase
        {
            private MockParentViewModel _child = new MockParentViewModel();
            public MockParentViewModel Child
            {
                get { return _child; }
            }
        }

        class MockParentViewModel2 : ViewModelBase
        {
            private MockChildViewModel _child = null;
            public MockChildViewModel Child
            {
                get { return _child; }
                set
                {
                    SetProperty( "Child", ref _child, value );
                }
            }
        }

        class MockParentWithTwoPropertiesOfSameType : ViewModelBase
        {
            private MockChildViewModel _child = null;
            public MockChildViewModel Child
            {
                get { return _child; }
                set
                {
                    SetProperty( "Child", ref _child, value );
                }
            }

            private MockChildViewModel _child2 = null;
            public MockChildViewModel Child2
            {
                get { return _child2; }
                set
                {
                    SetProperty( "Child2", ref _child2, value );
                }
            }
        }

        class MockViewModelWithForcedRevalidate : ViewModelBase
        {
            private int _trigger;
            public int Trigger
            {
                get { return _trigger; }
                set
                {
                    //NOTE: the trigger property specifies true for the final parameter (reValidate)
                    //The default for this parameter is false.  By setting true, this property indicates
                    //that all other properties on this ViewModel that have validation attributes should be
                    //re-validated.  It does this by raising OnPropertyChanged with the names of all the 
                    //other decorated properties.  
                    SetProperty( "Trigger", ref _trigger, value, true );
                }
            }

            private int _undecoratedValue;
            public int UndecoratedValue
            {
                get { return _undecoratedValue; }
                set
                {
                    SetProperty( "UndecoratedValue", ref _undecoratedValue, value );
                }
            }

            private int _decoratedValue;

            //The specific validation attribute doesn't matter here.  The fact that this property is decorated
            //means the base view model will detect it and refresh it when force re-validate occurs.
            [ExcludeEnum( Exclude=5, ResourceKey = "Error_Name_Not_Valid", ResourceManagerSource = typeof( TestResources ) ) ]
            public int DecoratedValue
            {
                get { return _decoratedValue; }
                set
                {
                    SetProperty( "DecoratedValue", ref _decoratedValue, value );
                }
            }
        }

        [TestMethod]
        public void ChangingChildPropertyRaisesDescendentChangeOnParent()
        {
            MockParentViewModel mock = new MockParentViewModel();
            bool propertyChangedCalled = false;
            string propertyChangedName = string.Empty;
            mock.DescendentPropertyChanged += ( sender, args ) =>
            {
                propertyChangedCalled = true;
                propertyChangedName = args.PropertyName;
            };
            Assert.IsFalse( propertyChangedCalled );

            mock.Child.Name = "meow";

            Assert.IsTrue( propertyChangedCalled );
            Assert.AreEqual( "Child.Name", propertyChangedName );
        }

        [TestMethod]
        public void ChangingChildPropertyRaisesDescendentChangeOnGrandParent()
        {
            MockGrandParentViewModel mock = new MockGrandParentViewModel();
            bool propertyChangedCalled = false;
            string propertyChangedName = string.Empty;
            mock.DescendentPropertyChanged += ( sender, args ) =>
            {
                propertyChangedCalled = true;
                propertyChangedName = args.PropertyName;
            };
            Assert.IsFalse( propertyChangedCalled );

            mock.Child.Child.Name = "meow";

            Assert.IsTrue( propertyChangedCalled );
            Assert.AreEqual( "Child.Child.Name", propertyChangedName );
        }

        [TestMethod]
        public void ModifyingPropertyOnDetachedObjectDoesNotRaisePropertyChangedEvent()
        {
            MockParentViewModel mock = new MockParentViewModel();
            MockChildViewModel child = mock.Child;
            mock.Child = null;

            bool propertyChangedCalled = false;
            mock.DescendentPropertyChanged += ( sender, args ) =>
            {
                propertyChangedCalled = true;
            };

            child.Name = "meow";
            Assert.IsFalse( propertyChangedCalled );
        }

        [TestMethod]
        public void ModifyingPropertyOnNewChildPropertyRaisesPropertyChangedEvent()
        {
            MockParentViewModel mock = new MockParentViewModel();
            MockChildViewModel child = new MockChildViewModel();

            mock.Child = child;

            bool propertyChangedCalled = false;
            mock.DescendentPropertyChanged += ( sender, args ) =>
            {
                propertyChangedCalled = true;
            };

            child.Name = "meow";
            Assert.IsTrue( propertyChangedCalled );
        }

        [TestMethod]
        public void ParentWithInitiallyNullChildConstructorWorks()
        {
            MockParentViewModel2 mock = new MockParentViewModel2();
        }

        [TestMethod]
        public void SettingChildCausesParentToReceiveChangeEvents()
        {
            MockParentViewModel2 mock = new MockParentViewModel2();
            var child = new MockChildViewModel();
            mock.Child = child;

            bool propertyChangedCalled = false;
            mock.DescendentPropertyChanged += ( sender, args ) =>
            {
                propertyChangedCalled = true;
            };

            child.Name = "meow";
            Assert.IsTrue( propertyChangedCalled );
        }

        [TestMethod]
        public void ViewModelWithDuplicateReferencesReceivesUpdatesForBoth()
        {
            var parent = new MockParentWithTwoPropertiesOfSameType();
            var child = new MockChildViewModel();

            parent.Child = child;
            parent.Child2 = child;

            bool childChanged = false;
            bool childChanged2 = false;
            parent.DescendentPropertyChanged += ( sender, args ) =>
            {
                if( args.PropertyName == "Child.Name" )
                    childChanged = true;
                else if( args.PropertyName == "Child2.Name" )
                    childChanged2 = true;
            };

            child.Name = "meow";
            Assert.IsTrue( childChanged );
            Assert.IsTrue( childChanged2 );
        }

        [TestMethod]
        public void ViewModelWithDuplicateReferencesComplexCase()
        {
            var parent = new MockParentWithTwoPropertiesOfSameType();
            var child = new MockChildViewModel();

            parent.Child = child;
            parent.Child2 = child;

            parent.Child2 = null;

            bool childChanged = false;
            bool childChanged2 = false;
            parent.DescendentPropertyChanged += ( sender, args ) =>
            {
                if( args.PropertyName == "Child.Name" )
                    childChanged = true;
                else if( args.PropertyName == "Child2.Name" )
                    childChanged2 = true;
            };

            child.Name = "meow";
            Assert.IsTrue( childChanged );
            Assert.IsFalse( childChanged2 );
        }

        [TestMethod]
        public void CallingSetPropertyWithRevalidateFlagRaisesPropertyChangedForDecoratedProperties()
        {
            var mock = new MockViewModelWithForcedRevalidate();
            bool decoratedPropertyChanged = false;
            bool undecoratedPropertyChanged = false;
            mock.PropertyChanged += ( sender, args ) =>
            {
                if( args.PropertyName == "UndecoratedValue" )
                    undecoratedPropertyChanged = true;

                if( args.PropertyName == "DecoratedValue" )
                    decoratedPropertyChanged = true;
            };

            Assert.IsFalse( decoratedPropertyChanged );
            Assert.IsFalse( undecoratedPropertyChanged );

            mock.Trigger = mock.Trigger;

            Assert.IsFalse( decoratedPropertyChanged );
            Assert.IsFalse( undecoratedPropertyChanged );

            mock.Trigger += 1;

            Assert.IsTrue( decoratedPropertyChanged );
            Assert.IsFalse( undecoratedPropertyChanged );
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzAspects;
using RzWpf;

namespace RzWpfTest
{
    [TestClass]
    public class WhenInheritingViewModelBaseGeneric
    {
        interface IMockModel
        {
            string Property_Name { get; }

            string Name { get; set; }
            int Id { get; set; }
        }

        class MockModel : ModelBase, IMockModel
        {
            public string Property_Name { get{ return "Name"; } }

            private string _name;
            public string Name 
            {
                get { return _name; }
                set { SetProperty( "Name", ref _name, value ); }
            }

            private int _id;
            public int Id
            {
                get { return _id; }
                set { SetProperty( "Id", ref _id, value ); }
            }
        }

        class MockChildViewModelGeneric : ViewModelBase<IMockModel>
        {
            public delegate void ContextImplDelegate( IMockModel context );

            public string Name
            {
                get { return Model.Name; }
                set { Model.Name = value; }
            }

            public int Id
            {
                get { return Model.Id; }
                set { Model.Id = value; }
            }
        }

        [TestMethod]
        public void ViewModelListensOnModelPropertyChangeEvent()
        {
            var vm = new MockChildViewModelGeneric();

            bool namePropertyChanged = false;
            vm.PropertyChanged += ( sender, args ) =>
            {
                if( "Name" == args.PropertyName )
                    namePropertyChanged = true;
            };

            var model = new MockModel();
            vm.Model = model;

            Assert.IsTrue( namePropertyChanged );
            namePropertyChanged = false;
            Assert.IsFalse( namePropertyChanged );
            vm.Model.Name = "meow";
            Assert.IsTrue( namePropertyChanged );
        }

        [TestMethod]
        public void ViewModelDoesNotListenOnUnsetModel()
        {
            var vm = new MockChildViewModelGeneric();
            bool namePropertyChanged = false;
            vm.PropertyChanged += ( sender, args ) =>
            {
                if( "Name" == args.PropertyName )
                    namePropertyChanged = true;
            };

            var model = new MockModel();
            vm.Model = model;
            vm.Model = null;

            Assert.IsTrue( namePropertyChanged );
            namePropertyChanged = false;
            Assert.IsFalse( namePropertyChanged );
            model.Name = "meow";
            Assert.IsFalse( namePropertyChanged );
        }

        [TestMethod]
        public void SettingModelCausesAllPropertiesToNotifyChange()
        {
            var vm = new MockChildViewModelGeneric(){ Model = new MockModel() };

            bool raiseChangeName = false;
            bool raiseChangeId = false;

            vm.PropertyChanged += ( sender, args ) =>
            {
                if( "Name" == args.PropertyName )
                    raiseChangeName = true;
                if( "Id" == args.PropertyName )
                    raiseChangeId = true;
            };

            Assert.IsFalse( raiseChangeName );
            Assert.IsFalse( raiseChangeId );

            vm.Model = new MockModel();

            Assert.IsTrue( raiseChangeName );
            Assert.IsTrue( raiseChangeId );
        }
    }
}
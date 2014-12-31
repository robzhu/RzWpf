using System;
using System.Reflection;
using System.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RzWpf.Localization;

namespace RzWpfTest
{
    [TestClass]
    public class WhenInheritingResourceStringAttribute
    {
        class MockResourceAttribute : ResourceStringAttribute
        {
        }
        
        class AttributedClass
        {
            [MockResourceAttribute( ResourceManagerSource = typeof( TestResources ) )]
            public string Name { get; set; }
        }

        class DerivedClass : AttributedClass
        {
        }

        class UnattributedClass
        {
        }

        [TestMethod]
        public void NewResourceStringAttributeHasNullType()
        {
            Assert.AreEqual( null, new MockResourceAttribute().ResourceManagerSource );
        }

        [TestMethod]
        public void NewResourceStringAttributeDoesNotContainGeneratedResourceType()
        {
            Assert.AreEqual( false, new MockResourceAttribute().IsGeneratedResourceType() );
        }

        [TestMethod]
        public void ParseNullTypeThrowsException()
        {
            try
            {
                ResourceStringAttribute.Parse( null );
                Assert.Fail( "should not reach here" );
            }
            catch( ArgumentNullException ) { }
        }

        [TestMethod]
        public void ParseReturnsAttributeForDecoratedProperty()
        {
            PropertyInfo propInfo = typeof( AttributedClass ).GetProperty( "Name" );
            Assert.IsNotNull( ResourceStringAttribute.Parse( propInfo ) );
        }

        [TestMethod]
        public void IsGeneratedResourceTypeWorksForPositiveCase()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceManagerSource = typeof( TestResources );
            Assert.IsTrue( attribute.IsGeneratedResourceType() );
        }

        [TestMethod]
        public void IsGeneratedResourceTypeWorksForNegativeCase()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceManagerSource = typeof( object );
            Assert.IsFalse( attribute.IsGeneratedResourceType() );
        }

        [TestMethod]
        public void DecoratedBaseClassDoesNotParseForDerivedClass()
        {
            Assert.IsNull( ResourceStringAttribute.Parse( typeof( DerivedClass ) ) );
        }

        [TestMethod]
        public void GetResourceManagerWorksForPositiveCase()
        {
            PropertyInfo propInfo = typeof( AttributedClass ).GetProperty( "Name" );
            var attribute = ResourceStringAttribute.Parse( propInfo );

            Assert.AreEqual( TestResources.ResourceManager, attribute.ResourceManager );
        }

        [TestMethod]
        public void NewResourceStringAttributeHasNullResourceKey()
        {
            var attribute = new MockResourceAttribute();

            Assert.AreEqual( null, attribute.ResourceKey );
        }

        [TestMethod]
        public void SettingResourceKeyToNullThrows()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceKey = "meow";
            try
            {
                attribute.ResourceKey = null;
                Assert.Fail( "should not reach here" );
            }
            catch( ArgumentException ) { }
        }

        [TestMethod]
        public void SettingResourceKeyToEmptyThrows()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceKey = "meow";
            try
            {
                attribute.ResourceKey = string.Empty;
                Assert.Fail( "should not reach here" );
            }
            catch( ArgumentException ) { }
        }

        [TestMethod]
        public void SettingResourceKeyToNonNullOrEmptyWorks()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceKey = "meow";
            Assert.AreEqual( "meow", attribute.ResourceKey );
        }

        [TestMethod]
        public void ValuePositiveCaseWorks()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceKey = "TestResource";
            attribute.ResourceManagerSource = typeof( TestResources );
            Assert.AreEqual( TestResources.TestResource, attribute.ResourceValue );
        }

        [TestMethod]
        public void NullResourceManagerThrowsDuringValidateResourceDescription()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceKey = "TestResource";
            attribute.ResourceManagerSource = null;
            try
            {
                attribute.ValidateResourceDescription();
                Assert.Fail( "should not reach here" );
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void MismatchingResourceKeyAndTypeThrowsDuringValidateResourceDescription()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceKey = "TestResource";
            attribute.ResourceManagerSource = typeof( object );
            try
            {
                attribute.ValidateResourceDescription();
                Assert.Fail( "should not reach here" );
            }
            catch (ArgumentException) { }
        }

        [TestMethod]
        public void NonExistentResourceKeyThrowsDuringValidateResourceDescription()
        {
            var attribute = new MockResourceAttribute();
            attribute.ResourceKey = "DoesNotExist";
            attribute.ResourceManagerSource = typeof( TestResources );
            try
            {
                attribute.ValidateResourceDescription();
                Assert.Fail( "should not reach here" );
            }
            catch (MissingManifestResourceException) { }
        }
    }
}

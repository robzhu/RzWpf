using System;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace RzWpf.Localization
{
    /// <summary>
    /// This attribute is used to specify a resource string defined in a resgen-generated resource manager.
    /// </summary>
    [AttributeUsage( AttributeTargets.Property )]
    public abstract class ResourceStringAttribute : Attribute, IResourceString
    {
        public const string ResGeneratedResourceManagerPropertyName = "ResourceManager";

        /// <summary>
        /// Parses the ResourceSourceAttribute from the specified type.
        /// </summary>
        /// <param name="decoratedItem">The field or type from which to parse the attribute.</param>
        /// <returns>The ResourceSourceAttribute</returns>
        public static ResourceStringAttribute Parse( ICustomAttributeProvider decoratedItem )
        {
            if( null == decoratedItem )
            {
                throw new ArgumentNullException( "decoratedItem" );
            }

            var attributes = decoratedItem.GetCustomAttributes( typeof( ResourceStringAttribute ), false );

            if( attributes.Length < 1 )
            {
                return null;
            }

            return attributes[ 0 ] as ResourceStringAttribute;
        }

        Type _type;
        string _resourceKey;
        PropertyInfo _resourceManagerProperty;

        public string ResourceKey
        {
            get { return _resourceKey; }
            set
            {
                if( _resourceKey != value )
                {
                    if( string.IsNullOrEmpty( value ) )
                        throw new ArgumentException( "resource key cannot be null or empty." );
                    _resourceKey = value;
                }
            }
        }

        /// <summary>
        /// Gets the ResourceManager defined by the Type property
        /// </summary>
        public ResourceManager ResourceManager
        {
            get
            {
                return ResourceManagerProperty.GetValue( null, null ) as ResourceManager;
            }
        }

        PropertyInfo ResourceManagerProperty
        {
            get
            {
                RefreshResourceManagerProperty();
                return _resourceManagerProperty;
            }
        }

        /// <summary>
        /// Gets or sets the type of the resource class.  Typically, this is the type of the class ResGen.exe generates from the resx files.
        /// </summary>
        public Type ResourceManagerSource
        {
            get { return _type; }
            set
            {
                if( _type != value )
                {
                    _type = value;
                    _resourceManagerProperty = null;
                    RefreshResourceManagerProperty();
                }
            }
        }

        public string ResourceValue
        {
            get
            {
                return ResourceManager.GetString( ResourceKey );
            }
        }

        /// <summary>
        /// Checks if the ResourceManagerSource property is a generated resource type.  
        /// </summary>
        /// <returns></returns>
        public bool IsGeneratedResourceType()
        {
            if( null == ResourceManagerSource )
            {
                return false;
            }

            if( null == ResourceManagerProperty )
            {
                return false;
            }

            return ( ResourceManagerProperty.PropertyType == typeof( ResourceManager ) );
        }

        private void RefreshResourceManagerProperty()
        {
            if( null == _resourceManagerProperty )
            {
                //ResGen-generated resource classes contain an internal static property used to retrieve the glocal resource manager instance.
                _resourceManagerProperty = ResourceManagerSource.GetProperty( ResGeneratedResourceManagerPropertyName,
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public );
            }
        }

        /// <summary>
        /// Checks to see if this attribute is correctly decorated by ensuring that the resource manager type is defined and correct,
        /// the resource key is defined and resolves to an actual value. 
        /// </summary>
        public void ValidateResourceDescription()
        {
            if( string.IsNullOrEmpty( ResourceKey ) )
            {
                throw new ArgumentException( "ResourceKey cannot be null or empty." );
            }

            if( null == ResourceManagerSource )
            {
                throw new ArgumentException( "type must be decorated with ResourceSourceAttribute" );
            }

            if( !IsGeneratedResourceType() )
            {
                throw new ArgumentException( "type's ResourceSourceAttribute did not specify a valid resource type" );
            }

            if( null == ResourceManager.GetString( ResourceKey ) )
            {
                throw new MissingManifestResourceException(
                    string.Format( Thread.CurrentThread.CurrentUICulture, "the resource key {0} does not resolve to a string in {1}", ResourceKey, ResourceManagerSource ) );
            }
        }
    }
}

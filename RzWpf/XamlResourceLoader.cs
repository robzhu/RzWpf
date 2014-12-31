using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RzWpf
{
    /// <summary>
    /// This class is responsible for loading DataTemplates from ResourceDictionary file.
    /// </summary>
    public class XamlResourceLoader
    {
        const string ResourceLocatorFormat = "/{0};component/{1}";
        const string PackResourceFormat = "pack://application:,,/{0};component/{1}";

        public static ResourceDictionary LoadComponent(string path)
        {
            return LoadComponent( Assembly.GetCallingAssembly().GetName().Name, path );
        }

        public static ResourceDictionary LoadComponent( string assemblyName, string path )
        {
            Uri resourceLocater = new Uri( String.Format( CultureInfo.InvariantCulture, ResourceLocatorFormat, assemblyName, path ), UriKind.Relative );
            ResourceDictionary rd = Application.LoadComponent( resourceLocater ) as ResourceDictionary;
            if( rd == null )
            {
                throw new ArgumentException( "The specified assembly and path did not resolve to a valid Resource", "path" );
            }

            return rd;
        }

        public static BitmapSource LoadBitmapSource( string path )
        {
            return LoadBitmapSource( Assembly.GetCallingAssembly().GetName().Name, path );
        }

        public static BitmapSource LoadBitmapSource( string assemblyName, string path )
        {
            return BitmapFrame.Create( BuildAbsoluteResourcePath( assemblyName, path ) );
        }

        public static Uri BuildAbsoluteResourcePath( string path )
        {
            return BuildAbsoluteResourcePath( Assembly.GetCallingAssembly().GetName().Name, path );
        }

        public static Uri BuildAbsoluteResourcePath( string assemblyName, string path )
        {
            return new Uri( String.Format( CultureInfo.InvariantCulture, PackResourceFormat, assemblyName, path ), UriKind.Absolute );
        }

        ResourceDictionary _rd = new ResourceDictionary();
        public XamlResourceLoader(string assemblyName, string path)
        {
            _rd = LoadComponent(assemblyName, path);
        }

        public XamlResourceLoader(string path)
            : this(Assembly.GetCallingAssembly().GetName().Name, path)
        {
        }

        /// <summary>
        /// Gets the DataTemplate object.
        /// </summary>
        /// <param name="key">Resource key.</param>
        /// <returns>DataTemplate object.</returns>
        public DataTemplate GetDataTemplate(string key)
        {
            return (DataTemplate)this._rd[key];
        }
    }
}

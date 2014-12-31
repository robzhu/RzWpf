using System;

namespace RzWpf.Localization
{
    /// <summary>
    /// This attribute is used to decorate enums so that they can be converted easily for data binding.
    /// </summary>
    [AttributeUsage( AttributeTargets.Field )]
    public sealed class DisplayStringAttribute : ResourceStringAttribute
    {
    }
}

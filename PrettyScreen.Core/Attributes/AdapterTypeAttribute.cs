using System;

namespace PrettyScreen.Core.Attributes
{
    public class AdapterTypeAttribute : Attribute
    {
        public AdapterType[] AdapterTypes { get; set; }

        public AdapterTypeAttribute(AdapterType adapterType)
        {
            AdapterTypes = new[] { adapterType };
        }

        public AdapterTypeAttribute(AdapterType adapterType, AdapterType adapterTypeSecond) : this(adapterType)
        {
            AdapterTypes = new[] { adapterType, adapterTypeSecond };
        }
    }
}

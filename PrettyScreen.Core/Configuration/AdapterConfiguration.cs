using PrettyScreen.Core;
using System;
using System.Collections.Generic;

namespace PrettyScreen.Configuration
{
    public class AdapterConfiguration : IUnique
    {
        public AdapterConfiguration()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public string IpAddress { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public AdapterType Type { get; set; }

        public IList<DataPointConfiguration> DataPoints { get; set; }
    }
}

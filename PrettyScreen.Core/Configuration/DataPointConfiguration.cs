using PrettyScreen.Core;
using System;

namespace PrettyScreen.Configuration
{
    public class DataPointConfiguration : IUnique
    {
        public DataPointConfiguration()
        {
        }

        public virtual AdapterConfiguration Adapter { get; set; }
        public Guid AdapterId { get; set; }
        public Guid Id { get; set; }
        public string Mapping { get; set; }
        public string Name { get; set; }
        public DataSource Source { get; set; }
        public TypeCode Type { get; set; }
        public string Unit { get; set; }
    }
}
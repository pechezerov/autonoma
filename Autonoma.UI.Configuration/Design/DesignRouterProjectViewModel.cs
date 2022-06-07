using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using System.Collections.Generic;
using System.ComponentModel;

namespace Autonoma.UI.Configuration.Design
{
    internal class DesignRouterProjectViewModel : IRouterProject
    {
        public IList<IRouter> Routers
            => new List<IRouter>()
            {
                new DesignRouter("XXX"),
                new DesignRouter("YYY"),
                new DesignRouter("ZZZ"),
            };
    }

    internal class DesignRouter : IRouter
    {
        public int? AdapterTypeId => 1;

        [Browsable(false)]
        public IList<IAdapter> Adapters
            => new List<IAdapter>()
            {
                new DesignAdapter("1"),
                new DesignAdapter("2"),
                new DesignAdapter("3"),
            };

        public string Name { get; set; }

        public DesignRouter(string name)
        {
            Name = name;
        }
    }

    internal class DesignAdapter : IAdapter
    {
        public string Name { get; set; }
        public AdapterType AdapterType { get; set; }

        public DesignAdapter(string name)
        {
            Name = name;
            AdapterType = new AdapterType();
        }
    }
}
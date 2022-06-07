using Autonoma.UI.Configuration.Abstractions;
using System.Collections.Generic;

namespace Autonoma.UI.Configuration.Design
{
    internal class DesignTopologyProjectViewModel : ITopologyProject
    {
        public IList<ILogicalNode> LogicalNodes
            => new List<ILogicalNode>()
            {
            };

        public IEnumerable<IModelElement> Elements => LogicalNodes;

        public string Name => "Test";
    }
}
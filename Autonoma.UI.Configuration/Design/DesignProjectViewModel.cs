using Autonoma.UI.Configuration.Abstractions;
using Dock.Model.ReactiveUI.Controls;

namespace Autonoma.UI.Configuration.Design
{
    public class DesignProjectViewModel : Document, IProject
    {
        public DesignProjectViewModel()
        {
        }

        public IRouterProject Technology => new DesignRouterProjectViewModel();

        public ITopologyProject Topology => new DesignTopologyProjectViewModel();

        public string? FilePath => null;
    }
}

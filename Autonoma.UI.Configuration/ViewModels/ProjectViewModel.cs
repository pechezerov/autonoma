using Autonoma.UI.Configuration.Abstractions;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI.Fody.Helpers;

namespace Autonoma.UI.Configuration.ViewModels
{
    public class ProjectViewModel : Document, IProject
    {
        public ProjectViewModel()
        {
            Technology = new RouterProjectViewModel();
            Topology = new TopologyProjectViewModel();
        }

        public string? FilePath { get; internal set; }

        [Reactive]
        public RouterProjectViewModel Technology { get; set; }
        IRouterProject IProject.Technology => Technology;

        [Reactive]
        public TopologyProjectViewModel Topology { get; set; }
        ITopologyProject IProject.Topology => Topology;
    }
}

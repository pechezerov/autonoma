using Autonoma.UI.Configuration.ViewModels;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Autonoma.UI.Configuration
{
    public interface IMainDockFactory : IFactory
    {
        public IDocumentDock MainDocumentDock { get; }

        void LoadProject(ProjectViewModel project);

        MainWindowViewModel MainContext { get; set; }
    }
}
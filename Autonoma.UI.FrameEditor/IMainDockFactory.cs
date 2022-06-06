using Autonoma.UI.FrameEditor.ViewModels;
using Autonoma.UI.FrameEditor.ViewModels.Documents;
using Autonoma.UI.Presentation.ViewModels;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace Autonoma.UI.FrameEditor
{
    public interface IMainDockFactory : IFactory
    {
        public IDocumentDock MainDocumentDock { get; }

        void LoadDocument(DocumentViewModel document);

        MainWindowViewModel MainContext { get; set; }
    }
}
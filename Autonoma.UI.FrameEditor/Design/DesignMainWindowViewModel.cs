using Autonoma.UI.FrameEditor.ViewModels;
using Autonoma.UI.Presentation.Infrastructure;
using Autonoma.UI.Presentation.Services;
using System.Linq;

namespace Autonoma.UI.FrameEditor.Design
{
    public class DesignMainWindowViewModel : MainWindowViewModel
    {
        public DesignMainWindowViewModel() : base()
        {
            CurrentDocument = new DesignDocumentViewModel();
            CurrentDocument.Frame.SelectedNode = CurrentDocument?.Frame.Nodes.First();
        }
    }
}

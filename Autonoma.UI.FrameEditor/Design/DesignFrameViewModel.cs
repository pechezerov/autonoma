using Autonoma.UI.FrameEditor.ViewModels.Documents;
using Autonoma.UI.Presentation.Design;

namespace Autonoma.UI.FrameEditor.Design
{
    public class DesignDocumentViewModel : DocumentViewModel
    {
        public DesignDocumentViewModel() : base(DesignFactory.CreateFrame("Test"))
        {
        }
    }
}

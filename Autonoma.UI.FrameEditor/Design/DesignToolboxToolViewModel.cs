using Autonoma.UI.FrameEditor.ViewModels.Tools;

namespace Autonoma.UI.FrameEditor.Design
{
    public class DesignToolboxToolViewModel : ToolboxToolViewModel
    {
        public DesignToolboxToolViewModel()
        {
            Context = new DesignMainWindowViewModel();
        }
    }
}

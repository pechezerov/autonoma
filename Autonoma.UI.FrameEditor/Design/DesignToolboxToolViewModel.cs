using Autonoma.UI.FrameEditor.ViewModels.Tools;
using Dock.Model.ReactiveUI.Controls;

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

using Autonoma.UI.FrameEditor.ViewModels.Tools;
using Dock.Model.ReactiveUI.Controls;

namespace Autonoma.UI.FrameEditor.Design
{
    public class DesignPropertiesToolViewModel : PropertiesToolViewModel
    {
        public DesignPropertiesToolViewModel()
        {
            Context = new DesignMainWindowViewModel();
        }
    }
}

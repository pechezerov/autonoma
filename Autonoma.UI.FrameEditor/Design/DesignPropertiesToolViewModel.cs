using Autonoma.UI.FrameEditor.ViewModels.Tools;

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

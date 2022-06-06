using Autonoma.UI.FrameEditor.ViewModels.Tools;
using Autonoma.UI.Presentation.Controls;
using Autonoma.UI.Presentation.ViewModels;
using Dock.Model.ReactiveUI.Controls;

namespace Autonoma.UI.FrameEditor.Design
{
    public class DesignElementViewModel : ElementViewModel
    {
        public DesignElementViewModel()
        {
            Content = new LampIndicatorControlViewModel();
        }
    }
}

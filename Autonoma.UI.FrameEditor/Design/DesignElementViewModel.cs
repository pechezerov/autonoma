using Autonoma.UI.Presentation.Controls;
using Autonoma.UI.Presentation.ViewModels;

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

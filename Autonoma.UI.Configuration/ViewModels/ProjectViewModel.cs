using Autonoma.UI.Presentation.ViewModels;
using Dock.Model.ReactiveUI.Controls;

namespace Autonoma.UI.FrameEditor.ViewModels
{
    public class ProjectViewModel : Document
    {
        public ProjectViewModel(FrameViewModel frame)
        {
        }

        public string FilePath { get; internal set; }
    }
}

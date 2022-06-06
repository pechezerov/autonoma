using Autonoma.UI.FrameEditor.ViewModels;
using Autonoma.UI.Presentation.Infrastructure;
using Autonoma.UI.Presentation.ViewModels;

namespace Autonoma.UI.Configuration.Abstractions
{
    public interface IProjectSerializer
    {
        string SerializeProject(ProjectViewModel value);

        ProjectViewModel DeserializeProject(string data);
    }
}

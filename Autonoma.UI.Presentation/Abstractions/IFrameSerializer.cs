using Autonoma.UI.Presentation.Infrastructure;
using Autonoma.UI.Presentation.ViewModels;

namespace Autonoma.UI.Presentation.Abstractions
{
    public interface IFrameSerializer
    {
        string Serialize<T>(T value);

        T? Deserialize<T>(string value);

        FrameViewModel DeserializeFrame(string data);

        ElementViewModel DeserializeElement(string data);
    }
}

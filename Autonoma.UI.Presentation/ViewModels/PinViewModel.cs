using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Model;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Autonoma.UI.Presentation.ViewModels
{
    [JsonObject]
    public class PinViewModel : ReactiveObject, IPin
    {
        public string? Name { get; set; }

        public IElement? Parent { get; set; }

        [Reactive]
        public double X { get; set; }

        [Reactive]
        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public PinAlignment Alignment { get; set; }

        public PinDirection Direction { get; set; }

        public virtual bool CanConnect()
        {
            return true;
        }

        public virtual bool CanDisconnect()
        {
            return true;
        }
    }
}

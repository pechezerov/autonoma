using Autonoma.UI.Presentation.Controls.Electric;
using Autonoma.UI.Presentation.Infrastructure;
using Autonoma.UI.Presentation.Services;
using Autonoma.UI.Presentation.ViewModels;

namespace Autonoma.UI.Presentation.Design
{
    public class DesignFactory
    {
        public static FrameViewModel CreateFrame(string title)
        {
            var factory = new ElementFactory();
            var frame = new FrameViewModel(new JsonFrameSerializer()) { Name = title, Width = 1000, Height = 500 };

            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 5; j++)
                {
                    var el = factory.CreateElement(typeof(SwitchControlViewModel));
                    el.Parent = frame;
                    el.X = i * 500;
                    el.Y = j * 100;
                    frame.Nodes.Add(el);
                }

            return frame;
        }
    }
}

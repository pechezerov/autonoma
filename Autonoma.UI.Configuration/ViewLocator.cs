using Autonoma.UI.Presentation;
using Autonoma.UI.Presentation.ViewModels;
using Avalonia.Controls;
using Dock.Model.Core;
using System;

namespace Autonoma.UI.Configuration
{
    public class EnhancedViewLocator : ViewLocator
    {
        public override IControl Build(object data)
        {
            var name = data.GetType().AssemblyQualifiedName!
                .Replace("ViewModel", "View");

            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + data.GetType().Name };
            }
        }

        public override bool Match(object data)
        {
            return data is ViewModelBase
                || data is IDockable; // аппендикс для того, чтобы загружались также представления менеджеров инструментов (Tools)
        }
    }
}

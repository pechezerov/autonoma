using Autonoma.UI.Operation.Views.Docking.Documents;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Core2D.ViewModels;
using Core2D.ViewModels.Docking.Documents;
using Dock.Model.ReactiveUI.Core;
using System;

namespace Autonoma.UI.Core2D.Editor
{
    public class ViewLocator : IDataTemplate
    {
        public ViewLocator()
        {

        }

        public virtual IControl Build(object data)
        {

            var name = data.GetType().AssemblyQualifiedName!
                .Replace("ControlViewModel", "Control")
                .Replace("ViewModel", "View");

            Type type;
            if (data.GetType() == typeof(PageViewModel))
                type = typeof(PageView);
            else
                type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public virtual bool Match(object data)
        {
            return data is ViewModelBase || data is DockableBase;
        }
    }
}

using Autonoma.UI.Presentation.ViewModels;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;

namespace Autonoma.UI.Presentation
{
    public class ViewLocator : IDataTemplate
    {
        public virtual IControl Build(object data)
        {
            var name = data.GetType().AssemblyQualifiedName!
                .Replace("ControlViewModel", "Control")
                .Replace("ViewModel", "View");
            var type = Type.GetType(name);

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
            return data is ViewModelBase;
        }
    }
}

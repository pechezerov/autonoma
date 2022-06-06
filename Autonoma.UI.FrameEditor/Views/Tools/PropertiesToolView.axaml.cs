using Autonoma.UI.Presentation.Attributes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using System.Reflection;

namespace Autonoma.UI.FrameEditor.Views.Tools
{
    public partial class PropertiesToolView : UserControl
    {
        public PropertiesToolView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public class PropertyViewModel : ReactiveObject
        {
            private readonly object source;
            private readonly PropertyInfo pi;

            public PropertyViewModel(PropertyInfo pi, object source)
            {
                this.pi = pi;
                this.source = source;
                IsConnectable = pi.GetCustomAttribute<ConnectedAttribute>() != null;
                Name = pi.Name;
                Value = pi.GetMethod?.Invoke(source, null);
            }

            public string Name { get; }

            public bool IsConnectable { get; }

            public object? Value
            {
                get
                {
                    return pi.GetMethod?.Invoke(source, null);
                }
                set
                {
                    try
                    {
                        pi.SetMethod?.Invoke(source, new[] { Convert.ChangeType(value, pi.PropertyType) });
                    }
                    catch { }
                }
            }

            public Array Values
            {
                get
                {
                    if (pi.PropertyType.IsEnum)
                        return Enum.GetValues(pi.PropertyType);
                    return Array.Empty<object>();
                }
            }
        }
    }
}

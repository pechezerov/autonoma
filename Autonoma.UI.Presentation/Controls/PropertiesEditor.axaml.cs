using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace Autonoma.UI.Presentation.Controls
{
    public partial class PropertiesEditor : UserControl
    {
        public PropertiesEditor()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static readonly StyledProperty<object?> EditedObjectProperty =
            AvaloniaProperty.Register<PropertiesEditor, object?>(nameof(EditedObject), null, false, Avalonia.Data.BindingMode.OneWay, 
                notifying: OnEditableObjectChanged);

        public object? EditedObject
        {
            get { return GetValue(EditedObjectProperty); }
            set { SetValue(EditedObjectProperty, value); }
        }

        private static void OnEditableObjectChanged(IAvaloniaObject arg1, bool arg2)
        {

        }
    }
}

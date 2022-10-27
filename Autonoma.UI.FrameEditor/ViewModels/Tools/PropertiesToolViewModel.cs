using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace Autonoma.UI.FrameEditor.ViewModels.Tools
{
    public class PropertiesToolViewModel : Tool
    {
        private object? _editedObject;

        public object? EditedObject
        {
            get => _editedObject;
            set => this.RaiseAndSetIfChanged(ref _editedObject, value);
        }
    }
}

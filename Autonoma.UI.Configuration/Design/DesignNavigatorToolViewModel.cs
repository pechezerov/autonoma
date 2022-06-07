using Autonoma.UI.Configuration.ViewModels.Tools;
using Dock.Model.ReactiveUI.Controls;

namespace Autonoma.UI.Configuration.Design
{
    public class DesignNavigatorToolViewModel : NavigatorToolViewModel
    {
        public DesignNavigatorToolViewModel()
        {
            Context = new DesignMainWindowViewModel();
        }
    }
}

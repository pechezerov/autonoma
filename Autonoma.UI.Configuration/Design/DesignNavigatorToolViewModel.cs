using Autonoma.UI.Configuration.ViewModels.Tools;

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

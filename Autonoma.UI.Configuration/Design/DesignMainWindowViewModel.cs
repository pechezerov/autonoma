using Autonoma.UI.Configuration.ViewModels;

namespace Autonoma.UI.Configuration.Design
{
    public class DesignMainWindowViewModel : MainWindowViewModel
    {
        public DesignMainWindowViewModel() : base(new MainDockFactory(), null)
        {
            Project = new DesignProjectViewModel();
        }
    }
}

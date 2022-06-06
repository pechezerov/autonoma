using Autonoma.App.ViewModels;
using Autonoma.Domain;
using Xamarin.Forms;

namespace Autonoma.App.Selectors
{
    public class DataPointDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MonitoringTemplate { get; set; }
        public DataTemplate ControlTemplate { get; set; }

        public DataPointDataTemplateSelector()
        {
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is DataPointDetailsViewModel dvm)
            {
                return dvm.Item.Source == DataSource.Field ? MonitoringTemplate : ControlTemplate;
            }
            return null;
        }
    }
}

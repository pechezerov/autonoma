using PrettyScreen.App.ViewModels;
using PrettyScreen.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrettyScreen.App.Selectors
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
                return dvm.DataPoint.Source == DataSource.Field ? MonitoringTemplate : ControlTemplate;
            }
            return null;
        }
    }
}

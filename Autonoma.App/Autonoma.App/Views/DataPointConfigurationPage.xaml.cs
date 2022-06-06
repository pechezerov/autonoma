using Autonoma.App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Autonoma.App.Views
{
    public partial class DataPointConfigurationPage : ContentPage
    {
        public DataPointConfigurationPage()
        {
            InitializeComponent();
            BindingContext = new DataPointConfigurationViewModel();
        }
    }
}
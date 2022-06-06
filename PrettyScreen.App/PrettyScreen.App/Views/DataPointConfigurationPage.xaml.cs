using PrettyScreen.App.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PrettyScreen.App.Views
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
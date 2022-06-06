using Autonoma.App.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Autonoma.App.Views
{
    public partial class AdapterDetailsPage : ContentPage
    {
        public AdapterDetailsPage()
        {
            InitializeComponent();
            BindingContext = new AdapterDetailsViewModel();
        }
    }
}
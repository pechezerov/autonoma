using PrettyScreen.App.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace PrettyScreen.App.Views
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
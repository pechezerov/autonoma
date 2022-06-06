using PrettyScreen.App.ViewModels;
using PrettyScreen.App.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PrettyScreen.App
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(DataPointConfigurationPage), typeof(DataPointConfigurationPage));
            Routing.RegisterRoute(nameof(AdapterDetailsPage), typeof(AdapterDetailsPage));
            Routing.RegisterRoute(nameof(DataPointDetailsPage), typeof(DataPointDetailsPage));
        }

    }
}

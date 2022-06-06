using Autonoma.App.ViewModels;
using Autonoma.App.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Autonoma.App
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

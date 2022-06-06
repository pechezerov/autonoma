using PrettyScreen.App.ViewModels;
using System;
using System.Timers;
using Xamarin.Forms;

namespace PrettyScreen.App.Views
{
    public partial class AdapterDiscoveryPage : ContentPage, IDisposable
    {
        AdapterDiscoveryViewModel viewModel;
        private bool disposedValue;
        private Timer refresher = new Timer(3000);

        public AdapterDiscoveryPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AdapterDiscoveryViewModel();
            refresher.Elapsed += RefreshAdapterStates;
            refresher.Start();
        }

        private void RefreshAdapterStates(object sender, ElapsedEventArgs e)
        {
            refresher.Enabled = false;
            try
            {
                viewModel.Update();
            }
            finally
            {
                refresher.Enabled = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshAdapterStates(null, null);
            refresher.Enabled = true;
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();
            refresher.Enabled = false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    refresher.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
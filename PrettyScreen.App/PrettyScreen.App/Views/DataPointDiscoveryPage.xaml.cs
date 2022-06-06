using PrettyScreen.App.ViewModels;
using System;
using System.Timers;
using Xamarin.Forms;

namespace PrettyScreen.App.Views
{
    public partial class DataPointDiscoveryPage : ContentPage, IDisposable
    {
        DataPointDiscoveryViewModel viewModel;
        private bool disposedValue;
        private Timer refresher = new Timer(3000);

        public DataPointDiscoveryPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DataPointDiscoveryViewModel();
            refresher.Elapsed += RefreshDataPoints;
            refresher.Start();
        }

        private void RefreshDataPoints(object sender, ElapsedEventArgs e)
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
            RefreshDataPoints(null, null);
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
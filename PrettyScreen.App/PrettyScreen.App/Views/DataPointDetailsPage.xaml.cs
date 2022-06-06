using PrettyScreen.App.ViewModels;
using System;
using System.Timers;
using Xamarin.Forms;

namespace PrettyScreen.App.Views
{
    public partial class DataPointDetailsPage : ContentPage, IDisposable
    {
        DataPointDetailsViewModel viewModel;
        private bool disposedValue;
        private Timer refresher = new Timer(3000);

        public DataPointDetailsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DataPointDetailsViewModel();
            refresher.Elapsed += RefreshDataPoint;
            refresher.Start();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshDataPoint(null, null);
            refresher.Enabled = true;
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();
            refresher.Enabled = false;
        }

        private void RefreshDataPoint(object sender, ElapsedEventArgs e)
        {
            viewModel.Update();
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
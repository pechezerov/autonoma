using PrettyScreen.App.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace PrettyScreen.App.ViewModels
{
    public class DataPointDiscoveryViewModel : BaseViewModel
    {
        public DataPointDiscoveryViewModel()
        {
            Title = "Browse";
            DataPoints = new ObservableCollection<DataPointDetailsViewModel>();
            LoadCommand = new Command(() => OnLoad());
            CreateCommand = new Command(() => OnCreate());
            SelectCommand = new Command<DataPointDetailsViewModel>(OnItemSelected);
        }

        public bool ExecuteUpdate { get; set; }

        public ObservableCollection<DataPointDetailsViewModel> DataPoints { get; set; }
        public Command SelectCommand { get; }
        public Command CreateCommand { get; }
        public Command LoadCommand { get; }
        public DataPointDetailsViewModel SelectedItem { get; set; }

        public void OnSelectedItemChanged()
        {
            OnItemSelected(SelectedItem);
        }

        private void OnCreate()
        {
            Shell.Current.GoToAsync($"{nameof(DataPointConfigurationPage)}?{nameof(DataPointConfigurationViewModel.Id)}={Guid.NewGuid()}");
        }

        private void OnLoad()
        {
            try
            {
                DataPoints.Clear();
                foreach (var dataPoint in DataPointService.DataPoints
                    .OrderBy(dp => dp.Name))
                {
                    var dataPointViewModel = new DataPointDetailsViewModel(dataPoint);
                    DataPoints.Add(dataPointViewModel);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnItemSelected(DataPointDetailsViewModel item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            Shell.Current.GoToAsync($"{nameof(DataPointDetailsPage)}?{nameof(DataPointDetailsViewModel.Id)}={item.Id}");
        }

        public void Update()
        {
            if (!DataPoints.Any())
            {
                OnLoad();
            }

            var items = DataPoints.ToArray();
            foreach (var dataPointViewModel in items)
            {
                dataPointViewModel.Update();
            }
        }
    }
}

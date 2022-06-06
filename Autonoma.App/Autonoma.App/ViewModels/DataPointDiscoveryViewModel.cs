using Autonoma.App.Models;
using Autonoma.App.Services.DataPoint;
using Autonoma.App.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Autonoma.App.ViewModels
{
    public class DataPointDiscoveryViewModel : BaseViewModel
    {
        public DataPointDiscoveryViewModel()
        {
            DataPoints = new ObservableCollection<DataPointInfo>();
            LoadCommand = new Command(async () => await OnLoad());
            CreateCommand = new Command(async () => await OnCreate());
            SelectCommand = new Command<DataPointInfo>(OnItemSelected);

            DataPointInfoService = DependencyService.Get<IDataPointInfoService>();
        }

        public IDataPointInfoService DataPointInfoService { get; }

        public Command SelectCommand { get; }
        public Command CreateCommand { get; }
        public Command LoadCommand { get; }

        public ObservableCollection<DataPointInfo> DataPoints { get; set; }
        public DataPointInfo SelectedItem { get; set; }

        public void OnSelectedItemChanged()
        {
            OnItemSelected(SelectedItem);
        }

        private async Task OnCreate()
        {
            await Shell.Current
                .GoToAsync($"{nameof(DataPointConfigurationPage)}?{nameof(DataPointInfo.Id)}={Guid.NewGuid()}");
        }

        private async Task OnLoad()
        {
            try
            {
                DataPoints.Clear();
                var dataPoints = await DataPointInfoService.GetAllDataPointsAsync();
                foreach (var dataPoint in dataPoints
                    .OrderBy(dp => dp.Name))
                {
                    DataPoints.Add(dataPoint);
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

        private void OnItemSelected(DataPointInfo item)
        {
            if (item == null)
                return;

            Shell.Current.GoToAsync($"{nameof(DataPointDetailsPage)}?{nameof(DataPointInfo.Id)}={item.Id}");
        }

        public async Task Update()
        {
            if (!DataPoints.Any())
            {
                await OnLoad();
            }

            // TODO: batch update
            foreach (var dataPoint in DataPoints)
            {
                var value = await DataPointInfoService.GetCurrentValueAsync(dataPoint.Id);
                dataPoint.Value = value;
            }
        }
    }
}

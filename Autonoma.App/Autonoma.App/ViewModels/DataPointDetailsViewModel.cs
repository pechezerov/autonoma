using Autonoma.App.Models;
using Autonoma.App.Services.DataPoint;
using Autonoma.App.Views;
using Autonoma.Core.Extensions;
using Autonoma.Core.MVVM.TaskCompletions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Autonoma.App.ViewModels
{
    public class DataPointDetailsViewModel : BaseViewModel
    {
        public IDataPointInfoService DataPointInfoService { get; }
        
        public Command ConfigureCommand { get; }
        public Command RemoveCommand { get; }

        public DataPointInfo Item { get; set; }

        public DataPointDetailsViewModel()
        {
            ConfigureCommand = new Command(() => NavigateToConfiguration());
            RemoveCommand = new Command(OnRemove);

            DataPointInfoService = DependencyService.Get<IDataPointInfoService>();
        }

        public override async Task InitializeAsync(IDictionary<string, string> query)
        {
            var dataPointId = query.GetValueAsInt(nameof(DataPointInfo.Id));

            if (dataPointId.ContainsKeyAndValue)
            {
                IsBusy = true;

                var authToken = SettingsService.AuthAccessToken;
                Item = await DataPointInfoService.GetDataPointByIdAsync(dataPointId.Value);
                Item.Value = await DataPointInfoService.GetCurrentValueAsync(dataPointId.Value);
                IsBusy = false;
            }
        }

        public async Task UpdateValueAsync()
        {
            if (Item != null)
            {
                IsBusy = true;

                var authToken = SettingsService.AuthAccessToken;
                Item.Value = await DataPointInfoService.GetCurrentValueAsync(Item.Id);

                IsBusy = false;
            }
        }

        private void NavigateToConfiguration()
        {
            Shell.Current.GoToAsync($"{nameof(DataPointConfigurationPage)}?{nameof(Item.Id)}={Item.Id}");
        }

        private void OnRemove(object obj)
        {
            DataPointInfoService.RemoveDataPointAsync(Item.Id);
            Shell.Current.GoToAsync("..");
        }
    }
}

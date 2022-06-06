using Autonoma.App.Models;
using Autonoma.App.Services.Adapter;
using Autonoma.Core.Extensions;
using Autonoma.Core.MVVM.TaskCompletions;
using Autonoma.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Autonoma.App.ViewModels
{
    public class AdapterDetailsViewModel : BaseViewModel
    {
        public IAdapterInfoService AdapterInfoService { get; }
        
        public Command StartCommand { get; }
        public Command StopCommand { get; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public AdapterType[] AllTypes => (AdapterType[])Enum.GetValues(typeof(AdapterType));

        public AdapterInfo Item { get; set; }

        public AdapterDetailsViewModel()
        {
            StartCommand = new Command(async () => await OnStart());
            StopCommand = new Command(async () => await OnStop());
            SaveCommand = new Command(async () => await OnSave());
            CancelCommand = new Command(async () => await OnCancel());

            AdapterInfoService = DependencyService.Get<IAdapterInfoService>();
        }

        public AdapterDetailsViewModel(AdapterInfo info) : this()
        {
            Item = info;
        }

        public override async Task InitializeAsync(IDictionary<string, string> query)
        {
            var dataPointId = query.GetValueAsInt(nameof(DataPointInfo.Id));

            if (dataPointId.ContainsKeyAndValue)
            {
                IsBusy = true;

                var authToken = SettingsService.AuthAccessToken;
                Item = await AdapterInfoService.GetAdapterByIdAsync(Item.Id);

                IsBusy = false;
            }
        }

        private async Task OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task OnSave()
        {
            if (Item == null)
                return;

            await AdapterInfoService.UpdateAdapterAsync(Item);

            await Shell.Current.GoToAsync("..");
        }

        private async Task OnStop()
        {
            await AdapterInfoService.StopAdapterByIdAsync(Item.Id);
        }

        private async Task OnStart()
        {
            await AdapterInfoService.StartAdapterByIdAsync(Item.Id);
        }
    }
}

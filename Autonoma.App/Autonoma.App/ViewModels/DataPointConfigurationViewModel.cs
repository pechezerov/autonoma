using Autonoma.App.Models;
using Autonoma.App.Services.DataPoint;
using Autonoma.Core;
using Autonoma.Core.Extensions;
using Autonoma.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Autonoma.App.ViewModels
{
    public class DataPointConfigurationViewModel : BaseViewModel
    {
        public DataPointInfo Edited { get; set; }

        public DataSource[] AllSourceTypes => (DataSource[])Enum.GetValues(typeof(DataSource));
        public TypeCode[] AllTypes => Globals.DataPointTypes.ToArray();

        public IDataPointInfoService DataPointInfoService { get; }

        public Command CancelCommand { get; }
        public Command LoadCommand { get; }
        public Command SaveCommand { get; }

        public DataPointConfigurationViewModel()
        {
            SaveCommand = new Command(async () => await OnSave());
            CancelCommand = new Command(async () => await OnCancel());

            DataPointInfoService = DependencyService.Get<IDataPointInfoService>();
        }

        public override async Task InitializeAsync(IDictionary<string, string> query)
        {
            var dataPointId = query.GetValueAsInt(nameof(DataPointInfo.Id));

            if (dataPointId.ContainsKeyAndValue)
            {
                IsBusy = true;

                var authToken = SettingsService.AuthAccessToken;
                Edited = await DataPointInfoService.GetDataPointByIdAsync(dataPointId.Value);

                IsBusy = false;
            }
        }

        private async Task OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async Task OnSave()
        {
            if (Validate())
            {
                await DataPointInfoService.CreateOrUpdateDataPointAsync(Edited);
                await Shell.Current.GoToAsync("..");
            }
        }

        private bool Validate()
        {
            return !String.IsNullOrWhiteSpace(Edited?.Name)
                && !String.IsNullOrWhiteSpace(Edited.Mapping) 
                && Edited.Id != null;
        }
    }
}

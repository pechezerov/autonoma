using Autonoma.App.Models;
using Autonoma.App.Services.Adapter;
using Autonoma.App.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Autonoma.App.ViewModels
{
    public class AdapterDiscoveryViewModel : BaseViewModel
    {
        public IAdapterInfoService AdapterInfoService { get; }

        public Command LoadCommand { get; }
        public Command AddCommand { get; }
        public Command RemoveCommand { get; }
        public Command ConfigureCommand { get; }

        public ObservableCollection<AdapterInfo> Items { get; }

        public AdapterDiscoveryViewModel()
        {
            Items = new ObservableCollection<AdapterInfo>();

            LoadCommand = new Command(async () => await OnLoad());
            AddCommand = new Command(async () => await OnAddItem());
            ConfigureCommand = new Command(async () => await OnConfigure(), () => SelectedItem != null);
            RemoveCommand = new Command(async () => await OnRemoveItem(), () => SelectedItem != null);

            AdapterInfoService = DependencyService.Get<IAdapterInfoService>();
        }

        public async Task Update()
        {
            if (Items == null || !Items.Any())
            {
                await OnLoad();
            }

            var states = await AdapterInfoService.GetAdapterStatesAsync();
            foreach (var item in Items)
            {
                item.State = states.FirstOrDefault(s => s.Id == SelectedItem.Id) ??
                    new AdapterStateInfo
                    {
                        Id = SelectedItem.Id,
                        State = Domain.WorkState.Error
                    };
            }
        }

        private async Task OnConfigure()
        {
            if (SelectedItem != null)
            {
                await Shell.Current
                     .GoToAsync($"{nameof(AdapterDetailsPage)}?{nameof(AdapterInfo.Id)}={SelectedItem.Id}");
            }
        }

        private async Task OnLoad()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await AdapterInfoService.GetAllAdaptersAsync();
                foreach (var item in items)
                {
                    Items.Add(item);
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

        public async Task OnAppearing()
        {
            await OnLoad();
            SelectedItem = null;
        }

        public AdapterInfo SelectedItem { get; set; }

        private void OnSelectedItemChanged()
        {
            ConfigureCommand.ChangeCanExecute();
            RemoveCommand.ChangeCanExecute();
        }

        private async Task OnAddItem()
        {
            await Shell.Current.GoToAsync($"{nameof(AdapterDetailsPage)}?{nameof(AdapterInfo.Id)}={Guid.NewGuid()}");
        }

        private async Task OnRemoveItem()
        {
            if (SelectedItem != null)
            {
                await AdapterInfoService.RemoveAdapterAsync(SelectedItem.Id);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
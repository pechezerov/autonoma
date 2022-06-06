using PrettyScreen.App.Views;
using PrettyScreen.Configuration;
using PrettyScreen.Core;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Xamarin.Forms;

namespace PrettyScreen.App.ViewModels
{
    public class AdapterDiscoveryViewModel : BaseViewModel
    {
        public ObservableCollection<AdapterDetailsViewModel> Items { get; }
        public Command LoadCommand { get; }
        public Command AddCommand { get; }
        public Command RemoveCommand { get; }
        public Command ConfigureCommand { get; }

        public AdapterDiscoveryViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<AdapterDetailsViewModel>();
            LoadCommand = new Command(() => OnLoad());

            AddCommand = new Command(() => OnAddItem());
            ConfigureCommand = new Command(() => OnConfigure(), () => SelectedItem != null);
            RemoveCommand = new Command(() => OnRemoveItem(), () => SelectedItem != null);
        }

        internal void Update()
        {
            if (Items == null || !Items.Any())
            {
                OnLoad();
            }

            foreach (var item in Items)
            {
                item.State = item.Adapter?.State ?? WorkState.Error ;
            }
        }

        private void OnConfigure()
        {
            if (SelectedItem != null)
                Shell.Current.GoToAsync($"{nameof(AdapterDetailsPage)}?{nameof(AdapterDetailsViewModel.Id)}={SelectedItem.Id}");
        }

        void OnLoad()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = AdapterConfigs.Items.Select(a => new AdapterDetailsViewModel(a.Id))
                    .ToList();
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

        public void OnAppearing()
        {
            OnLoad();
            SelectedItem = null;
        }

        public AdapterDetailsViewModel SelectedItem { get; set; }

        private void OnSelectedItemChanged()
        {
            ConfigureCommand.ChangeCanExecute();
            RemoveCommand.ChangeCanExecute();
        }

        private void OnAddItem()
        {
            Shell.Current.GoToAsync($"{nameof(AdapterDetailsPage)}?{nameof(AdapterDetailsViewModel.Id)}={Guid.NewGuid()}");
        }

        private void OnRemoveItem()
        {
            if (SelectedItem != null)
            {
                Guid id = Guid.Parse(SelectedItem.Id);
                DataPointConfigs.DeleteItem(id);
                DataPointService.RemoveDataPoint(id);
                Shell.Current.GoToAsync("..");
            }
        }
    }
}
using PrettyScreen.Configuration;
using PrettyScreen.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Xamarin.Forms;

namespace PrettyScreen.App.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class AdapterDetailsViewModel : BaseViewModel
    {
        public Command StartCommand { get; }
        public Command StopCommand { get; }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command LoadCommand { get; }

        public AdapterType[] AllTypes => (AdapterType[])(Enum.GetValues(typeof(AdapterType)));
        public IDataAdapter Adapter { get; set; }
        public AdapterConfiguration Configuration { get; set; }

        public WorkState State { get; set; }

        public AdapterDetailsViewModel()
        {
            StartCommand = new Command(OnStart);
            StopCommand = new Command(OnStop);
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            LoadCommand = new Command(OnLoad);
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            if (Configuration == null)
                return;

            AdapterConfigs.UpdateItem(Configuration);
            await Shell.Current.GoToAsync("..");
        }

        private void OnLoad(object obj)
        {
            LoadAdapterById(Id);
        }

        private void OnStop(object obj)
        {
            Adapter.Stop();
        }

        private void OnStart(object obj)
        {
            Adapter.Start();
        }

        public AdapterDetailsViewModel(Guid id) : this()
        {
            Id = id.ToString();
        }

        public string Id { get; set; }

        public void OnIdChanged()
        {
            LoadAdapterById(Id);
        }

        public void LoadAdapterById(string sid)
        {
            try
            {
                Guid id = Guid.Parse(sid);
                Adapter = AdapterService.Adapters.FirstOrDefault(a => a.Id == id);
                if (Adapter != null)
                {
                    Configuration = AdapterConfigs.GetItem(id);
                }
                
                if (Configuration == null)
                {
                    Configuration = new AdapterConfiguration() { Id = id };
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}

using PrettyScreen.Configuration;
using PrettyScreen.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace PrettyScreen.App.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class DataPointConfigurationViewModel : BaseViewModel
    {
        public DataPointConfigurationViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            LoadCommand = new Command(OnLoad);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command LoadCommand { get; }

        public DataPointConfiguration Edited { get; set; }

        public TypeCode[] AllTypes => Globals.DataPointTypes.ToArray();
        public DataSource[] AllSourceTypes => (DataSource[])Enum.GetValues(typeof(DataSource));
        public IDataAdapter[] AllAdapters => AdapterService.Adapters.ToArray();

        private void OnCancel()
        {
            Shell.Current.GoToAsync("..");
        }

        private void OnSave()
        {
            if (Validate())
            {
                DataPointConfigs.UpdateItem(Edited);
                var dp = DataPointService.GetDataPoint(Edited.Id);
                DataPointService.CreateOrUpdateDataPoint(Edited, true);

                Shell.Current.GoToAsync("..");
            }
        }

        private bool Validate()
        {
            return !String.IsNullOrWhiteSpace(Edited?.Name)
                && !String.IsNullOrWhiteSpace(Edited?.Mapping) 
                && Edited?.AdapterId != null;
        }

        private void OnLoad(object obj)
        {
            LoadById(Id);
        }

        public IDataAdapter Adapter { get; set; }

        public void OnAdapterChanged()
        {
            if (Edited != null)
                Edited.AdapterId = Adapter?.Id ?? Globals.IdleAdapterId;
        }

        public string Id { get; set; }

        public void OnIdChanged()
        {
            LoadById(Id);
        }

        public void LoadById(string sid)
        {
            IsBusy = true;

            try
            {
                Guid id = Guid.Parse(sid);
                var storedConfig = DataPointConfigs.GetItem(id);

                if (storedConfig == null)
                {
                    Edited = new DataPointConfiguration { Name = "Новый тег" };
                    Adapter = null;
                }
                else
                {
                    Edited = storedConfig;
                    Adapter = AdapterService.Adapters.FirstOrDefault(a => a.Id == Edited.AdapterId);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

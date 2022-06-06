using PrettyScreen.App.Views;
using PrettyScreen.Configuration;
using PrettyScreen.Core;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace PrettyScreen.App.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class DataPointDetailsViewModel : BaseViewModel
    {
        public DataPointDetailsViewModel()
        {
            ConfigureCommand = new Command(() => NavigateToConfiguration());
            RemoveCommand = new Command(OnRemove);
        }

        public DataPointDetailsViewModel(IDataPoint dataPoint) : this()
        {
            DataPoint = dataPoint;
            Id = dataPoint?.Id.ToString();
        }

        public Command ConfigureCommand { get; }
        public Command RemoveCommand { get; }

        public IDataPoint DataPoint { get; set; }

        public DataValue Current { get; set; }

        private string id;
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                LoadItemByPath(value);
            }
        }
        private void NavigateToConfiguration()
        {
            Shell.Current.GoToAsync($"{nameof(DataPointConfigurationPage)}?{nameof(DataPointConfigurationViewModel.Id)}={Id}");
        }

        private void OnRemove(object obj)
        {
            Guid id = Guid.Parse(Id);
            DataPointConfigs.DeleteItem(id); 
            DataPointService.RemoveDataPoint(id);
            Shell.Current.GoToAsync("..");
        }

        public void Update()
        {
            if (DataPoint == null)
            {
                LoadItemByPath(Id);
            }

            if (DataPoint != null)
            {
                var actualValue = DataPoint.Current with { };
                if (actualValue != Current)
                    Current = actualValue;
            }
        }

        public void LoadItemByPath(string sid)
        {
            try
            {
                Guid id = Guid.Parse(sid);
                DataPoint = DataPointService.GetDataPoint(id);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}

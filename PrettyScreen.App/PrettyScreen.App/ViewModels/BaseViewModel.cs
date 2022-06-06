using PrettyScreen.Configuration;
using PrettyScreen.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace PrettyScreen.App.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<DataPointConfiguration> DataPointConfigs => DependencyService.Get<IDataStore<DataPointConfiguration>>();
        public IDataStore<AdapterConfiguration> AdapterConfigs => DependencyService.Get<IDataStore<AdapterConfiguration>>();
        public IDataPointService DataPointService => DependencyService.Get<IDataPointService>();
        public ICommunicationService AdapterService => DependencyService.Get<ICommunicationService>();

        public bool IsBusy { get; set; } = false;

        public string Title { get; set; } = string.Empty;

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

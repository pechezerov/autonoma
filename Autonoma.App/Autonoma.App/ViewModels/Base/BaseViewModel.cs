using Autonoma.App.Services.Dialog;
using Autonoma.App.Services.Navigation;
using Autonoma.App.Services.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Autonoma.App.ViewModels
{
    public abstract class BaseViewModel : ExtendedBindableObject, IQueryAttributable
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly ISettingsService SettingsService;

        private bool _isInitialized;

        public bool IsInitialized
        {
            get => _isInitialized;

            set
            {
                _isInitialized = value;
                OnPropertyChanged(nameof(IsInitialized));
            }
        }

        private bool _multipleInitialization;

        public bool MultipleInitialization
        {
            get => _multipleInitialization;

            set
            {
                _multipleInitialization = value;
                OnPropertyChanged(nameof(MultipleInitialization));
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;

            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public BaseViewModel()
        {
            DialogService = ViewModelLocator.Resolve<IDialogService>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
            SettingsService = ViewModelLocator.Resolve<ISettingsService>();

            GlobalSetting.Instance.BaseIdentityEndpoint = SettingsService.IdentityEndpointBase;
        }

        public virtual Task InitializeAsync(IDictionary<string, string> query)
        {
            return Task.FromResult(false);
        }

        public async void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
                await InitializeAsync(query);
            }
        }
    }
}

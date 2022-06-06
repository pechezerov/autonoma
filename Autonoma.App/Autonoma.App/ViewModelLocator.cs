using Autonoma.App.Services.Adapter;
using Autonoma.App.Services.DataPoint;
using Autonoma.App.Services.RequestProvider;
using Autonoma.App.Services.Settings;
using Autonoma.Communication.Hosting;
using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace Autonoma.App
{
    public static class ViewModelLocator
    {
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        public static bool UseMockService { get; set; }

        static ViewModelLocator()
        {
            var settingsService = new SettingsService();
            DependencyService.RegisterSingleton<ISettingsService>(settingsService);

            var useRemoteConnection = settingsService.UseRemoteConnection;

            if (useRemoteConnection)
            {
                var requestProvider = new RequestProvider();
                var dataPointInfoService = new DataPointInfoService(requestProvider, settingsService);
                var adapterInfoService = new AdapterInfoService(requestProvider, settingsService);

                DependencyService.RegisterSingleton<IRequestProvider>(requestProvider);
                DependencyService.RegisterSingleton<IDataPointInfoService>(dataPointInfoService);
                DependencyService.RegisterSingleton<IAdapterInfoService>(adapterInfoService);
            }
            else
            {
                var dataPointHostService = new DataPointHostService(settingsService.SystemName);
                var adapterHostService = new AdapterHostService(dataPointHostService);
                var dataPointInfoService = new DataPointLocalInfoService(dataPointHostService);
                var adapterInfoService = new AdapterLocalInfoService(adapterHostService);

                DependencyService.RegisterSingleton<IDataPointInfoService>(dataPointInfoService);
                DependencyService.RegisterSingleton<IAdapterInfoService>(adapterInfoService);
                DependencyService.RegisterSingleton<IDataPointHostService>(dataPointHostService);
                DependencyService.RegisterSingleton<IAdapterHostService>(adapterHostService);
            }

            // View models - by default, TinyIoC will register concrete classes as multi-instance.
            //DependencyService.Register<BasketViewModel>();
        }

        public static T Resolve<T>() where T : class
        {
            return DependencyService.Get<T>();
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }

            var viewModel = Activator.CreateInstance(viewModelType);

            view.BindingContext = viewModel;
        }
    }
}

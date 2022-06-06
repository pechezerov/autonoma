using PrettyScreen.App.ViewModels;
using PrettyScreen.Configuration;
using PrettyScreen.Configuration.Repositories;
using PrettyScreen.Core;
using PrettyScreen.Core.Util;
using System.Linq;
using Xamarin.Forms;

namespace PrettyScreen.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            var configStore = new ConfigurationRepository();
            if (!configStore.GetDataPoints().Any())
            {
                new ConfigurationContextSeedDataGenerator().Seed();
            }
            var dataService = new SimpleDataPointService(configStore);
            var commService = new SimpleCommunicationService(dataService, configStore);

            DependencyService.RegisterSingleton<IDataStore<DataPointConfiguration>>(configStore);
            DependencyService.RegisterSingleton<IDataStore<AdapterConfiguration>>(configStore);
            DependencyService.RegisterSingleton<IDataPointService>(dataService);
            DependencyService.RegisterSingleton<ICommunicationService>(commService);

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

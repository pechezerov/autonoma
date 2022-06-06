using IO.Swagger.Api;

namespace Autonoma.API.Main.Client
{
    public class MainApi : IMainApi
    {
        public MainApi(IAdaptersConfigurationApi adaptersConfigurationApi, IDataPointsApi dataPointsApi, IDataPointsConfigurationApi dataPointsConfigurationApi)
        {
            AdaptersConfigurationApi = adaptersConfigurationApi;
            DataPointsApi = dataPointsApi;
            DataPointsConfigurationApi = dataPointsConfigurationApi;
        }

        public IAdaptersConfigurationApi AdaptersConfigurationApi { get; set; }
        public IDataPointsApi DataPointsApi { get; set; }
        public IDataPointsConfigurationApi DataPointsConfigurationApi { get; set; }
    }
}

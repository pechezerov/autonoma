using IO.Swagger.Api;

namespace Autonoma.API.Main.Client
{
    public interface IMainApi
    {
        IAdaptersConfigurationApi AdaptersConfigurationApi { get; set; }
        IDataPointsApi DataPointsApi { get; set; }
        IDataPointsConfigurationApi DataPointsConfigurationApi { get; set; }
    }
}

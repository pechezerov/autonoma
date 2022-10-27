using Autofac;
using Autonoma.API.Infrastructure;
using Autonoma.Communication.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Autonoma.API
{
    internal class DataPointServiceStartupInitializer : IStartable
    {
        private readonly IDataPointService _dps;
        private readonly IUnitOfWork _uow;

        public DataPointServiceStartupInitializer(IConfiguration configuration, IUnitOfWork uow, IDataPointService dps)
        {
            _dps = dps;
            _uow = uow;
        }

        public void Start()
        {
            if (_dps is IHostedService hdps)
               hdps.StartAsync(default);
        }
    }
}
using Autofac;
using Autonoma.API.Infrastructure;
using Autonoma.Communication.Hosting;
using Microsoft.Extensions.Configuration;
using System.Linq;

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
            _dps.Initialize(_uow.DataPointRepository.All().ToList());
        }
    }
}
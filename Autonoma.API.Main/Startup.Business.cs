using Autonoma.API.Infrastructure;
using Autonoma.Configuration;
using Autonoma.Configuration.Repositories;
using Autonoma.Configuration.Repositories.Abstractions;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autonoma.API
{
    public static class StartupBusinnessExtensions
    {
		public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
			services
				.AddTransient<IUnitOfWork, UnitOfWork>()
				.AddTransient<IGenericRepository<AdapterConfiguration>, AdapterConfigurationRepository>()
                .AddTransient<IGenericRepository<DataPointConfiguration>, DataPointConfigurationRepository>();
		}
    }
}

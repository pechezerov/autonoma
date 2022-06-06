using Autonoma.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autonoma.Communication.ModbusTCP.Client
{
    public static class StartupBusinnessExtensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string cs = configuration.GetConnectionString("ConfigurationDatabase");
            services.AddDbContext<ConfigurationContext>(cfg =>
            {
                cfg.UseSqlite(cs)
                    .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
                    .EnableSensitiveDataLogging();
            });
            return services;
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}

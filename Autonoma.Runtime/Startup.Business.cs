using Autonoma.Configuration;
using Autonoma.ProcessManagement;
using Autonoma.ProcessManagement.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Autonoma.Runtime
{
    public static class StartupBusinnessExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            string cs = configuration.GetConnectionString("ConfigurationDatabase");
            services.AddDbContext<ConfigurationContext>(cfg =>
            {
                cfg.UseSqlite(cs)
                    .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
                    .EnableSensitiveDataLogging();
            });

            services.AddDbContext<ProcessManagementContext>(cfg =>
            {
                cfg.UseInMemoryDatabase(databaseName: $"{nameof(Autonoma)}{nameof(Autonoma.ProcessManagement)}")
                    .UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }))
                    .EnableSensitiveDataLogging();
            });
        }

        public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IProcessCollector, ProcessCollector>();
            services.AddSingleton<IProcessManager, ProcessManager>();
        }
    }
}

using AutoMapper;
using Autonoma.API.Main.Client;
using Autonoma.Communication.Hosting;
using Autonoma.Communication.Hosting.Remote;
using Autonoma.Communication.Infrastructure;
using Autonoma.Communication.Modbus;
using Autonoma.Configuration;
using Autonoma.Core.Infrastructure;
using Autonoma.Domain.Entities;
using IO.Swagger.Api;
using IO.Swagger.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autonoma.Communication.ModbusTCP.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var commandLineSwitchMappings = new Dictionary<string, string>()
             {
                 { "-I", "AdapterRunSettings:AdapterId" },
                 { "-M", "AdapterRunSettings:MainApiUrl" }
             };

            var appConfiguration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddCommandLine(args, commandLineSwitchMappings)
                .AddEnvironmentVariables()
                .Build();

            var adapterId = appConfiguration.GetValue<int>("AdapterRunSettings:AdapterId");
            var mainApiUrl = appConfiguration.GetValue<string>("AdapterRunSettings:MainApiUrl");

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    // configuration file
                    services.AddSingleton<IConfiguration>(appConfiguration)
                            .AddCustomDbContext(appConfiguration)
                            // mapper configuration modules
                            .AddSingleton<IMapperConfiguration, ContractMapperConfiguration>()
                            // mapper configuration module registration method
                            .AddSingleton<AdapterConfiguration>(provider =>
                            {
                                var mapper = provider
                                    .GetRequiredService<IMapper>();
                                var adapterConfigurationApi = provider
                                    .GetRequiredService<IAdaptersConfigurationApi>();
                                var adapterConfigurationResult = adapterConfigurationApi
                                    .AdaptersConfigurationIdGet(adapterId);
                                var adapterConfiguration = mapper.Map<AdapterConfiguration>(adapterConfigurationResult.Adapter);
                                return adapterConfiguration;
                            })
                            // mapper configuration module registration method
                            .AddSingleton<IMapper>(provider =>
                            {
                                var mapper = new MapperConfiguration(config =>
                                {
                                    foreach (var mapping in provider.GetServices<IMapperConfiguration>())
                                        mapping.Configure(config);
                                });
                                return mapper.CreateMapper();
                            })
                            // communication services
                            .AddSingleton<IAdaptersConfigurationApi>(new AdaptersConfigurationApi(mainApiUrl))
                            .AddSingleton<IDataPointsApi>(new DataPointsApi(mainApiUrl))
                            .AddSingleton<IDataPointsConfigurationApi>(new DataPointsConfigurationApi(mainApiUrl))
                            .AddSingleton<IDataPointService, RemoteService>()
                            .AddSingleton<IMainApi, MainApi>()
                            .AddSingleton<ModbusClient>()
                )
                .Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            var modbusClient = provider.GetRequiredService<ModbusClient>();
        }
    }
}

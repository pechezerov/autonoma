using AutoMapper;
using Autonoma.API.Main.Client;
using Autonoma.Communication.Hosting;
using Autonoma.Communication.Hosting.Remote;
using Autonoma.Communication.Infrastructure;
using Autonoma.Communication.Modbus;
using Autonoma.Core.Infrastructure;
using Autonoma.Domain.Entities;
using IO.Swagger.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Canceling...");
                cts.Cancel();
                e.Cancel = true;
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
                            // mapper configuration modules
                            .AddSingleton<IMapperConfiguration, ContractMapperConfiguration>()
                            // mapper configuration module registration method
                            .AddSingleton<AdapterConfiguration>(provider =>
                            {
                                while (!cts.IsCancellationRequested)
                                {
                                    try
                                    {
                                        var mapper = provider.GetRequiredService<IMapper>();
                                        var adapterConfigurationApi = provider
                                            .GetRequiredService<IAdaptersConfigurationApi>();
                                        var adapterConfigurationResult = adapterConfigurationApi
                                            .AdaptersConfigurationIdGet(adapterId);
                                        var adapterConfiguration = mapper.Map<AdapterConfiguration>(adapterConfigurationResult.Adapter);
                                        Console.WriteLine("Подключение установлено");
                                        return adapterConfiguration;
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Не удалось подключиться к серверу, ожидание...");
                                    }
                                }
                                throw new TaskCanceledException();
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

            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var modbusClient = provider.GetRequiredService<ModbusClient>();
                    var workTask = modbusClient.StartAsync(cts.Token);
                    Task.WaitAll(new[] { workTask });
                }
                catch (Exception)
                {
                    Console.WriteLine("Не удалось подключиться к серверу, ожидание...");
                }
            }
        }
    }
}

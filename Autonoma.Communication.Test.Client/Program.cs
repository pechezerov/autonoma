﻿using AutoMapper;
using Autonoma.API.Main.Client;
using Autonoma.Communication.Hosting;
using Autonoma.Communication.Hosting.Remote;
using Autonoma.Communication.Infrastructure;
using Autonoma.Core.Infrastructure;
using Autonoma.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Autonoma.Communication.Test.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (s, e) =>
            {
                Console.WriteLine("Canceling...");
                cts.Cancel();
                e.Cancel = true;
            };

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
                            // mapper configuration modules
                            .AddSingleton<IMapperConfiguration, ContractMapperConfiguration>()
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
                            // adapter configuration
                            .AddSingleton<AdapterConfiguration>(provider =>
                            {
                                while (!cts.IsCancellationRequested)
                                {
                                    try
                                    {
                                        var mapper = provider
                                            .GetRequiredService<IMapper>();
                                        var apiMainClient = provider
                                            .GetRequiredService<APIMainClient>();
                                        var adapterConfigurationResult = (apiMainClient
                                            .AdaptersConfiguration_AdapterConfigurationByIdAsync(adapterId)).Result;
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

                            // communication services
                            .AddSingleton<IDataPointService, RemoteService>()
                            .AddSingleton<APIMainClient, APIMainClient>()
                            .AddHostedService<TestClient>()
                )
                .Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            await host.RunAsync(cts.Token);
        }
    }
}

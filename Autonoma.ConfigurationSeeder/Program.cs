using Autonoma.Communication.Test.Client;
using Autonoma.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Autonoma.ConfigurationSeeder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var appConfiguration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                   {
                       // configuration file
                       services.AddSingleton<IConfiguration>(appConfiguration)
                               .AddCustomDbContext(appConfiguration);
                   })
                .Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var seeder = new ConfigurationSeeder(provider.GetRequiredService<ConfigurationContext>());
            seeder.Seed();
        }
    }
}

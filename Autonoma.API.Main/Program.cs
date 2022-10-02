using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Autonoma.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
                })
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

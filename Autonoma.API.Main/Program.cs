using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autonoma.API.Main.Infrastructure;
using Autonoma.API.Shared.Infrastructure;
using Autonoma.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Autonoma.API
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new BusinessModule()));

            builder.Services.AddControllers(opts =>
                {
                    // Post-handling exceptions
                    opts.Filters.Add<ExceptionFilter>();
                })
                // NewtonsoftJson used because it can
                // map an undefined type to object.
                // STJ in this case creates JsonElement.
                // This is an important aspect in the point exchange scenario.
                .AddNewtonsoftJson();

            // add database contexts
            builder.Services.AddCustomDbContext(builder.Configuration, LoggerFactory.Create(builder => { builder.AddConsole(); }));
            // add services
            builder.Services.AddBusinessServices(builder.Configuration);
            // configure mapper
            builder.Services.AddCustomMapper(builder.Configuration);
            // configure swagger
            builder.Services.AddCustomSwagger();
            builder.Services.AddSignalR()
                .AddNewtonsoftJsonProtocol();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DataRouterHub>("/Data");
            });

            app.Run();
        }
    }
}

using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Autonoma.Configuration;
using Autonoma.API.Main.Infrastructure;
using Microsoft.Extensions.Logging;
using Autonoma.Model.Akka.Services;

namespace Autonoma.API
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                // NewtonsoftJson used because it can
                // map an undefined type to object.
                // STJ in this case creates JsonElement.
                // This is an important aspect in the point exchange scenario.
                .AddNewtonsoftJson();

            // add database contexts
            services.AddCustomDbContext(Configuration, LoggerFactory.Create(builder => { builder.AddConsole(); }));
            // add services
            services.AddBusinessServices(Configuration);
            // configure mapper
            services.AddCustomMapper(Configuration);
            // configure swagger
            services.AddCustomSwagger();

            services.AddSignalR()
                .AddNewtonsoftJsonProtocol();

            // starts the IHostedService, which creates the ActorSystem and actors
            services.AddTransient<IHostedService, AkkaService>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessModule>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DataRouterHub>("/Data");
            });
        }
    }
}

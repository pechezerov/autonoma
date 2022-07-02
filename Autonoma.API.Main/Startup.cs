using Autofac;
using Autonoma.API.Main.Infrastructure;
using Autonoma.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autonoma.API
{
    public partial class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                // NewtonsoftJson used because it can
                // map an undefined type to object.
                // STJ in this case creates JsonElement.
                // This is an important aspect in the point exchange scenario.
                .AddNewtonsoftJson();

            // add database contexts
            services.AddCustomDbContext(_configuration, LoggerFactory.Create(builder => { builder.AddConsole(); }));
            // add services
            services.AddBusinessServices(_configuration);
            // configure mapper
            services.AddCustomMapper(_configuration);
            // configure swagger
            services.AddCustomSwagger();

            services.AddSignalR()
                .AddNewtonsoftJsonProtocol();
        }

        public static void ConfigureContainer(ContainerBuilder builder)
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

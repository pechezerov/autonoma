using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Autonoma.API
{
    public static class StartupSwaggerExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = $"{nameof(Autonoma)}.{nameof(Autonoma.API)}.{nameof(Autonoma.API.Main)}", Version = "v1" });
            }).AddSwaggerGenNewtonsoftSupport();
        }
    }
}

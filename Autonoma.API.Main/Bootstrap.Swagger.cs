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
                c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");
                c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = $"{nameof(Autonoma)}.{nameof(Autonoma.API)}.{nameof(Autonoma.API.Main)}", Version = "v1" });
            }).AddSwaggerGenNewtonsoftSupport();
        }
    }
}

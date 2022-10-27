using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Autonoma.Runtime.Infrastructure;
using Autonoma.Core.Infrastructure;

namespace Autonoma.Runtime
{
    public static class StartupMappingExtensions
    {
        public static void AddCustomMapper(this IServiceCollection services)
        {
            services
                .AddSingleton<IMapperConfiguration, ApiMapperConfiguration>();

            services.AddSingleton(provider =>
            {
                var mapper = new MapperConfiguration(config =>
                {
                    foreach (var mapping in provider.GetServices<IMapperConfiguration>())
                    {
                        mapping.Configure(config);
                    }
                });
                return mapper.CreateMapper();
            });
        }
    }
}

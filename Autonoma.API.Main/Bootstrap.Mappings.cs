using AutoMapper;
using Autonoma.Communication.Infrastructure;
using Autonoma.Core.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Autonoma.API
{
    public static class StartupMappingExtensions
    {
        public static void AddCustomMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IMapperConfiguration, ContractMapperConfiguration>();

            services.AddSingleton<IMapper>(provider =>
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

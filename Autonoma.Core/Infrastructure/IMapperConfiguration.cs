using AutoMapper;

namespace Autonoma.Core.Infrastructure
{
    public interface IMapperConfiguration
    {
        void Configure(IMapperConfigurationExpression config);
    }
}
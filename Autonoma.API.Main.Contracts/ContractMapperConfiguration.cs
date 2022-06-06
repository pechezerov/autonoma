using AutoMapper;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.Core.Infrastructure;
using Autonoma.Domain.Entities;

namespace Autonoma.Communication.Infrastructure
{
    public class ContractMapperConfiguration : IMapperConfiguration
    {
        public void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<AdapterType, AdapterTypeItem>()
                .ReverseMap();
            config.CreateMap<DataPointConfiguration, DataPointConfigurationItem>()
                .ReverseMap();
            config.CreateMap<AdapterConfiguration, AdapterConfigurationItem>()
                .IncludeMembers()
                .ReverseMap();
        }
    }
}
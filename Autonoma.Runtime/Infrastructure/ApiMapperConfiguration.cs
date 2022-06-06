using AutoMapper;
using Autonoma.Core.Infrastructure;
using Autonoma.ProcessManagement;
using Autonoma.Runtime.Queries.Process;

namespace Autonoma.Runtime.Infrastructure
{
    public class ApiMapperConfiguration : IMapperConfiguration
    {
        public void Configure(IMapperConfigurationExpression config)
        {
            // mappings between DTOs and domain objects
            config.CreateMap<ProcessBase, ProcessItem>();
        }
    }
}
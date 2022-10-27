using AutoMapper;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.API.Queries;
using Autonoma.Domain.Entities;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.DataPoint
{
    public class DataPointConfigurationByIdQueryHandler : QueryHandlerAsync<DataPointConfigurationByIdQuery, DataPointConfigurationByIdQueryResult>
    {
        private readonly IMapper _mapper;

        public DataPointConfigurationByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
             : base(uow)
        {
            this._mapper = mapper;
        }

        public override async Task<DataPointConfigurationByIdQueryResult> ExecuteAsync(DataPointConfigurationByIdQuery query)
        {
            var entity = await _uow.DataPointRepository.FindAsync(query.Id);
            return new DataPointConfigurationByIdQueryResult 
            { 
                DataPoint = _mapper.Map<DataPointConfiguration, DataPointConfigurationItem>(entity)
            };
        }
    }
}

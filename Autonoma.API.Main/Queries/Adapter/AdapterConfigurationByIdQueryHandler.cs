using AutoMapper;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Queries;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.Adapter
{
    public class AdapterConfigurationByIdQueryHandler : QueryHandlerAsync<AdapterConfigurationByIdQuery, AdapterConfigurationByIdQueryResult>
    {
        private readonly IMapper _mapper;

        public AdapterConfigurationByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
             : base(uow)
        {
            this._mapper = mapper;
        }

        public override async Task<AdapterConfigurationByIdQueryResult> ExecuteAsync(AdapterConfigurationByIdQuery query)
        {
            var idsToSelect = new[] { query.Id };

            var item = await _uow.AdapterRepository
                .AllIncludeAsQueryable(a => a.DataPoints, a => a.AdapterType)
                .Where(ci => idsToSelect.Contains(ci.Id))
                .Select(dp => _mapper.Map<AdapterConfiguration, AdapterConfigurationItem>(dp))
                .FirstOrDefaultAsync();

            return new AdapterConfigurationByIdQueryResult
            {
                Adapter = item
            };
        }
    }
}

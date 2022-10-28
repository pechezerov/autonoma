using AutoMapper;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Queries;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.Adapter
{
    public class AdapterConfigurationListQueryHandler : QueryHandlerAsync<AdapterConfigurationListQuery, AdapterConfigurationListQueryResult>
    {
        private readonly IMapper _mapper;

        public AdapterConfigurationListQueryHandler(IUnitOfWork uow, IMapper mapper)
             : base(uow)
        {
            this._mapper = mapper;
        }

        public override async Task<AdapterConfigurationListQueryResult> ExecuteAsync(AdapterConfigurationListQuery query)
        {
            var relevantItemsCount = await _uow.AdapterRepository.AllAsQueryable()
                .Where(ci => !query.Ids.Any() || query.Ids.Contains(ci.Id))
                .LongCountAsync();

            var itemsOnPage = _uow.AdapterRepository
                .AllIncludeAsQueryable(a => a.DataPoints)
                .Where(ci => !query.Ids.Any() || query.Ids.Contains(ci.Id))
                .OrderBy(c => c.Name)
                .Skip(query.PageSize * (query.PageIndex - 1))
                .Take(query.PageSize);

            var result = await itemsOnPage
                .Select(dp => _mapper.Map<AdapterConfiguration, AdapterConfigurationItem>(dp))
                .ToListAsync();

            return new AdapterConfigurationListQueryResult(query.PageIndex, query.PageSize, relevantItemsCount, result);
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Autonoma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autonoma.API.Infrastructure;
using Autonoma.API.Queries;
using Autonoma.API.Main.Contracts.Adapter;

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
            if (query.Ids.Any())
            {
                var totalItems = await _uow.AdapterRepository.AllAsQueryable()
                    .LongCountAsync();

                var itemsOnPage = _uow.AdapterRepository
                    .AllIncludeAsQueryable(a => a.DataPoints)
                    .OrderBy(c => c.Name)
                    .Skip(query.PageSize * (query.PageIndex - 1))
                    .Take(query.PageSize);

                var result = await itemsOnPage
                    .Select(dp => _mapper.Map<AdapterConfiguration, AdapterConfigurationItem>(dp))
                    .ToListAsync();

                return new AdapterConfigurationListQueryResult(query.PageIndex, query.PageSize, totalItems, result);
            }
            else
            {
                var relevantItems = await GetItemsByIdsAsync(query.Ids);
                return new AdapterConfigurationListQueryResult(1, relevantItems.Count, relevantItems.Count, relevantItems);
            }
        }

        private async Task<List<AdapterConfigurationItem>> GetItemsByIdsAsync(IEnumerable<int> ids)
        {
            var items = await _uow.AdapterRepository
                .AllIncludeAsQueryable(a => a.DataPoints, a => a.AdapterType)
                .Where(ci => !ids.Any() || ids.Contains(ci.Id))
                .Select(dp => _mapper.Map<AdapterConfiguration, AdapterConfigurationItem>(dp))
                .ToListAsync();

            return items;
        }
    }
}

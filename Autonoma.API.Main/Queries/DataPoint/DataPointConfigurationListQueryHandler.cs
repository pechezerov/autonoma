using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Autonoma.API.Main.Infrastructure;
using Autonoma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autonoma.API.Queries;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;

namespace Autonoma.API.Main.Queries.DataPoint
{
    public class DataPointConfigurationListQueryHandler : QueryHandlerAsync<DataPointConfigurationListQuery, DataPointConfigurationListQueryResult>
    {
        private readonly IMapper _mapper;

        public DataPointConfigurationListQueryHandler(IUnitOfWork uow, IMapper mapper)
             : base(uow)
        {
            this._mapper = mapper;
        }

        public override async Task<DataPointConfigurationListQueryResult> ExecuteAsync(DataPointConfigurationListQuery query)
        {
            if (query.Ids == null)
            {
                var totalItems = await _uow.DataPointRepository.AllAsQueryable()
                    .LongCountAsync();

                var itemsOnPage = await _uow.DataPointRepository.AllAsQueryable()
                    .OrderBy(c => c.Name)
                    .Skip(query.PageSize * (query.PageIndex - 1))
                    .Take(query.PageSize)
                    .Select(dp => _mapper.Map<DataPointConfiguration, DataPointConfigurationItem>(dp))
                    .ToListAsync();

                return new DataPointConfigurationListQueryResult(query.PageIndex, query.PageSize, totalItems, itemsOnPage);
            }
            else
            {
                var relevantItems = await GetItemsByIdsAsync(query.Ids);
                return new DataPointConfigurationListQueryResult(1, relevantItems.Count, relevantItems.Count, relevantItems);
            }
        }

        private async Task<List<DataPointConfigurationItem>> GetItemsByIdsAsync(IEnumerable<int> ids)
        {
            if (ids == null || !ids.Any())
                return new List<DataPointConfigurationItem>();

            var items = await _uow.DataPointRepository.AllAsQueryable()
                .Where(ci => ids.Contains(ci.Id))
                .Select(dp => _mapper.Map<DataPointConfiguration, DataPointConfigurationItem>(dp))
                .ToListAsync();

            return items;
        }
    }
}

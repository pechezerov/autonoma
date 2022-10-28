using AutoMapper;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.API.Queries;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var relevantItemsCount = await _uow.DataPointRepository
                .AllAsQueryable()
                .Where(ci => !query.Ids.Any() || query.Ids.Contains(ci.Id))
                .LongCountAsync();

            var itemsOnPage = _uow.DataPointRepository
                .AllAsQueryable()
                .Where(ci => !query.Ids.Any() || query.Ids.Contains(ci.Id))
                .OrderBy(c => c.Name)
                .Skip(query.PageSize * (query.PageIndex - 1))
                .Take(query.PageSize);

            var result = await itemsOnPage
                .Select(dp => _mapper.Map<DataPointConfiguration, DataPointConfigurationItem>(dp))
                .ToListAsync();

            return new DataPointConfigurationListQueryResult(query.PageIndex, query.PageSize, relevantItemsCount, result);
        }
    }
}

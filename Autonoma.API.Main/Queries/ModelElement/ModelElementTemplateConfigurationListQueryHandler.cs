using AutoMapper;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.ModelElementTemplate;
using Autonoma.API.Queries;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.ModelElement
{
    public class ModelElementTemplateConfigurationListQueryHandler : QueryHandlerAsync<ModelElementTemplateConfigurationListQuery, ModelElementTemplateConfigurationListQueryResult>
    {
        private readonly IMapper _mapper;

        public ModelElementTemplateConfigurationListQueryHandler(IUnitOfWork uow, IMapper mapper)
             : base(uow)
        {
            _mapper = mapper;
        }

        public override async Task<ModelElementTemplateConfigurationListQueryResult> ExecuteAsync(ModelElementTemplateConfigurationListQuery query)
        {
            var relevantItemsCount = await _uow.AdapterRepository.AllAsQueryable()
                .Where(ci => !query.Ids.Any() || query.Ids.Contains(ci.Id))
                .LongCountAsync();

            var itemsOnPage = _uow.ModelTemplateRepository
                .AllIncludeAsQueryable(a => a.Attributes)
                .Where(ci => !query.Ids.Any() || query.Ids.Contains(ci.Id))
                .OrderBy(c => c.Name)
                .Skip(query.PageSize * (query.PageIndex - 1))
                .Take(query.PageSize);

            var result = await itemsOnPage
                .Select(dp => _mapper.Map<ModelElementTemplateConfiguration, ModelElementTemplateConfigurationItem>(dp))
                .ToListAsync();

            return new ModelElementTemplateConfigurationListQueryResult(query.PageIndex, query.PageSize, relevantItemsCount, result);
        }
    }
}

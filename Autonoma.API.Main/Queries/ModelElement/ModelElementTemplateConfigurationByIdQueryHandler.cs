using AutoMapper;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.ModelElementTemplate;
using Autonoma.API.Queries;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.ModelElement
{
    public class ModelElementTemplateConfigurationByIdQueryHandler : QueryHandlerAsync<ModelElementTemplateConfigurationByIdQuery, ModelElementTemplateConfigurationByIdQueryResult>
    {
        private readonly IMapper _mapper;

        public ModelElementTemplateConfigurationByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
             : base(uow)
        {
            _mapper = mapper;
        }

        public override async Task<ModelElementTemplateConfigurationByIdQueryResult> ExecuteAsync(ModelElementTemplateConfigurationByIdQuery query)
        {
            var idsToSelect = new[] { query.Id };

            var item = await _uow.ModelTemplateRepository
                .AllIncludeAsQueryable(a => a.Attributes)
                .Where(ci => idsToSelect.Contains(ci.Id))
                .Select(dp => _mapper.Map<ModelElementTemplateConfiguration, ModelElementTemplateConfigurationItem>(dp))
                .FirstAsync();

            return new ModelElementTemplateConfigurationByIdQueryResult
            {
                ModelElementTemplateConfiguration = item
            };
        }
    }
}

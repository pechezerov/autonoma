using Autonoma.Configuration.Repositories;
using Autonoma.Configuration.Repositories.Abstractions;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autonoma.ConfigurationManagement.Repositories
{
    public class ModelElementTemplateConfigurationRepository : GenericRepository<ModelElementTemplateConfiguration>, IModelElementTemplateRepository
    {
        public ModelElementTemplateConfigurationRepository(DbContext context) : base(context)
        {
        }
    }
}

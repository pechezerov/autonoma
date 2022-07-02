using Autonoma.Configuration.Repositories;
using Autonoma.Configuration.Repositories.Abstractions;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autonoma.ConfigurationManagement.Repositories
{
    public class ModelElementConfigurationRepository : GenericRepository<ModelElementConfiguration>, IModelElementRepository
    {
        public ModelElementConfigurationRepository(DbContext context) : base(context)
        {
        }
    }
}

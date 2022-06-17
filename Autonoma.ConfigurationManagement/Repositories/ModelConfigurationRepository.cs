using Autonoma.Configuration.Repositories;
using Autonoma.Configuration.Repositories.Abstractions;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autonoma.ConfigurationManagement.Repositories
{
    public class ModelConfigurationRepository : GenericRepository<ModelElementConfiguration>, IModelRepository
    {
        public ModelConfigurationRepository(DbContext context) : base(context)
        {
        }
    }
}

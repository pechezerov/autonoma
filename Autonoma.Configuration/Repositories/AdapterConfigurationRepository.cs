using Autonoma.Configuration.Repositories.Abstractions;
using Autonoma.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autonoma.Configuration.Repositories
{

    public class AdapterConfigurationRepository : GenericRepository<AdapterConfiguration>, IAdapterRepository
    {
        public AdapterConfigurationRepository(DbContext context) : base(context)
        {
        }
    }
}

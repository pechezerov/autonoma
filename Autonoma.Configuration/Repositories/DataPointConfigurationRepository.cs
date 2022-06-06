using Microsoft.EntityFrameworkCore;
using Autonoma.Configuration.Repositories.Abstractions;
using Autonoma.Domain.Entities;

namespace Autonoma.Configuration.Repositories
{
    public class DataPointConfigurationRepository : GenericRepository<DataPointConfiguration>, IDataPointRepository
    {
        public DataPointConfigurationRepository(DbContext context) : base(context)
        {
        }
    }
}

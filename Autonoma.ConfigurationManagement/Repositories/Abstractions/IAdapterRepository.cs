using Autonoma.Domain.Entities;

namespace Autonoma.Configuration.Repositories.Abstractions
{
    public interface IAdapterRepository : IGenericRepository<AdapterConfiguration>
    {
    }

    public interface IModelRepository : IGenericRepository<ModelElementConfiguration>
    {

    }
}

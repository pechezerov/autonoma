using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Autonoma.Configuration.Repositories.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);
        Task CreateAsync(TEntity item);

        void Update(TEntity item);

        void Delete(int id);

        TEntity Find(int id);
        Task<TEntity> FindAsync(int id);

        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();

        IQueryable<TEntity> AllAsQueryable();
        IEnumerable<TEntity> AllInclude(params Expression<Func<TEntity, object>>[] includeProperties);
    }
}

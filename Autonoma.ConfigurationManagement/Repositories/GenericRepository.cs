using Microsoft.EntityFrameworkCore;
using Autonoma.Configuration.Exceptions;
using Autonoma.Configuration.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Autonoma.Configuration.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbContext _context;
        protected DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        #region Read

        public IEnumerable<TEntity> All()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public IQueryable<TEntity> AllAsQueryable() => _dbSet;

        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IQueryable<TEntity> AllIncludeAsQueryable (params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetAllIncluding(false, includeProperties);
        }

        private IQueryable<TEntity> GetAllIncluding(bool trackingEnabled, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = trackingEnabled ? _dbSet : _dbSet.AsNoTracking();

            return includeProperties.Aggregate
              (queryable, (current, includeProperty) => current.Include(includeProperty));
        }

        public TEntity Find(int id)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            var entity = _dbSet.AsNoTracking().SingleOrDefault(lambda);

            if (entity == null)
                throw new EntityNotFoundException<TEntity>(id);
            return entity;
        }

        public async Task<TEntity> FindAsync(int id)
        {
            Expression<Func<TEntity, bool>> lambda = Utilities.BuildLambdaForFindByKey<TEntity>(id);
            var entity = await _dbSet.AsNoTracking().SingleOrDefaultAsync(lambda);

            if (entity == null)
                throw new EntityNotFoundException<TEntity>(id);
            return entity;
        }

        #endregion

        #region Create

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        #endregion

        #region Update

        public void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        #endregion

        #region Delete

        public void Delete(int id)
        {
            var entity = Find(id);
            _dbSet.Remove(entity);
        }

        #endregion
    }
}

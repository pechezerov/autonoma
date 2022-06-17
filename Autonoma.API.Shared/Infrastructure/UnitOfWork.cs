using Autonoma.Configuration;
using Autonoma.Configuration.Repositories;
using Autonoma.Configuration.Repositories.Abstractions;
using Autonoma.ConfigurationManagement.Repositories;
using System;
using System.Threading.Tasks;

namespace Autonoma.API.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _disposed;

        private readonly ConfigurationContext _context;

        public IAdapterRepository AdapterRepository { get; }
        public IDataPointRepository DataPointRepository { get; }
        public IModelRepository ModelRepository { get; }

        public UnitOfWork(ConfigurationContext context)
        {
            _context = context;

            AdapterRepository = new AdapterConfigurationRepository(_context);
            DataPointRepository = new DataPointConfigurationRepository(_context);
            ModelRepository = new ModelConfigurationRepository(_context);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

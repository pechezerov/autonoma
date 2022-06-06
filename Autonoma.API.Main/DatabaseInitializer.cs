using Autofac;
using Autonoma.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Autonoma.API
{
    internal class DatabaseInitializer : IStartable
    {
        private readonly IConfiguration _configuration;
        private readonly ConfigurationContext _context;

        public DatabaseInitializer(IConfiguration configuration, ConfigurationContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public void Start()
        {
            if (_configuration.GetValue<bool>("CleanDatabase"))
            {
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }

            if (_configuration.GetValue<bool>("MigrateDatabase"))
                _context.Database.Migrate();
        }
    }
}
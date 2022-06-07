using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using Autonoma.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Autonoma.Configuration
{
    public class ConfigurationContext : DbContext
    {
        public string ConnectionString { get; private set; }

        public DbSet<AdapterConfiguration> Adapters { get; set; }

        public DbSet<AdapterType> AdapterTypes { get; set; }

        public DbSet<DataPointConfiguration> DataPoints { get; set; }

        public DbSet<User> Users { get; set; }

        public ConfigurationContext(DbContextOptions<ConfigurationContext> options) : base(options)
        {
            SqliteOptionsExtension? sqliteOptions =
                   options.FindExtension<SqliteOptionsExtension>();
            if (sqliteOptions != null)
            {
                ConnectionString = sqliteOptions.ConnectionString;
            }

            SQLitePCL.Batteries_V2.Init();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdapterConfiguration>()
                .HasKey(a => a.Id);
            modelBuilder.Entity<AdapterConfiguration>()
                .HasMany(a => a.DataPoints)
                .WithOne(p => p.Adapter)
                .HasForeignKey(s => s.AdapterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DataPointConfiguration>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<User>().HasOne(o => o.CreatedBy).WithMany();
            modelBuilder.Entity<User>().HasOne(o => o.ModifiedBy).WithMany();
        }
    }

    public static class StartupBusinnessExtensions
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerfactory)
        {
            string cs = configuration.GetConnectionString("ConfigurationDatabase");
            services.AddDbContext<ConfigurationContext>(cfg =>
            {
                cfg.UseSqlite(cs)
                    .UseLoggerFactory(loggerfactory)
                    .EnableSensitiveDataLogging();
            });
        }
    }
}

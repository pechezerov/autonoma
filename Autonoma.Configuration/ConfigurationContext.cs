using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Infrastructure.Internal;
using Autonoma.Domain.Entities;

namespace Autonoma.Configuration
{
    public class ConfigurationContext : DbContext
    {
        public string ConnectionString { get; private set; }

        public DbSet<AdapterConfiguration> Adapters { get; set; }

        public DbSet<AdapterType> AdapterTypes { get; set; }

        public DbSet<DataPointConfiguration> DataPoints { get; set; }

        public ConfigurationContext(DbContextOptions<ConfigurationContext> options) : base(options)
        {
            var sqlServerOptionsExtension =
                   options.FindExtension<SqliteOptionsExtension>();
            if (sqlServerOptionsExtension != null)
            {
                ConnectionString = sqlServerOptionsExtension.ConnectionString;
            }

            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
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
}

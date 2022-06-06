using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace PrettyScreen.Configuration
{
    public class ConfigurationContext : DbContext
    {
        public DbSet<AdapterConfiguration> Adapters { get; set; }
        public DbSet<DataPointConfiguration> DataPoints { get; set; }

        public ConfigurationContext()
        {
            SQLitePCL.Batteries_V2.Init();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "configuration.db3");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdapterConfiguration>()
            .HasMany(a => a.DataPoints)
            .WithOne(p => p.Adapter)
            .HasForeignKey(s => s.AdapterId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AdapterConfiguration>().HasKey(a => a.Id);
            modelBuilder.Entity<DataPointConfiguration>().HasKey(a => a.Id);
        }
    }
}

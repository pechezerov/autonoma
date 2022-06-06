using Autonoma.Core;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using System;
using System.Linq;

namespace Autonoma.Configuration
{
    public class ConfigurationSeeder
    {
        private readonly ConfigurationContext _context;

        public ConfigurationSeeder(ConfigurationContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();

            CheckTestAdapters();
            CheckTestDataPoints();        
        }

        private void CheckTestAdapters()
        {
            if (_context.Adapters.Any())
                return;

            var idleAdapterType = new AdapterType
            {
                Id = Globals.IdleAdapterTypeId,
                Name = "Пассивный"
            };
            _context.AdapterTypes.Add(idleAdapterType);
            _context.Adapters.Add(new AdapterConfiguration
            {
                Id = Globals.IdleAdapterId,
                IpAddress = "",
                Name = "Пассивный",
                AdapterTypeId = idleAdapterType.Id
            });

            var testAdapterType = new AdapterType
            {
                Id = Globals.TestAdapterTypeId,
                Name = "Тестовый"
            };
            _context.AdapterTypes.Add(idleAdapterType);
            _context.Adapters.Add(new AdapterConfiguration
            {
                Id = Globals.TestAdapterId,
                IpAddress = "",
                Name = "Тестовый",
                AdapterTypeId = testAdapterType.Id
            });

            _context.SaveChanges();
        }

        private void CheckTestDataPoints()
        {
            if (_context.DataPoints.Any())
                return;

            for (int i = 0; i < 5; i++)
            {
                _context.DataPoints.Add(new DataPointConfiguration
                {
                    Name = $"Variable{i}",
                    Type = TypeCode.Double,
                    AdapterId = Globals.TestAdapterId,
                    Mapping = $"mappingparams({i})"
                });
            }
            for (int i = 0; i < 2; i++)
            {
                _context.DataPoints.Add(new DataPointConfiguration
                {
                    Name = $"Command{i}",
                    Type = TypeCode.Boolean,
                    Source = DataSource.Control,
                    AdapterId = Globals.TestAdapterId,
                    Mapping = $"mappingparams({i})"
                });
            }

            _context.SaveChanges();
        }
    }
}

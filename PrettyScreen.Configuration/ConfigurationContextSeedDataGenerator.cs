using PrettyScreen.Core;
using PrettyScreen.Core.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrettyScreen.Configuration
{
    public class ConfigurationContextSeedDataGenerator
    {
        public void Seed()
        {
            using (var ctx = new ConfigurationContext())
            {
                ctx.Adapters.Add(new AdapterConfiguration
                {
                    Id = Globals.IdleAdapterId,
                    IpAddress = "",
                    Name = "Пассивный",
                    Type = AdapterType.Idle
                });

                ctx.Adapters.Add(new AdapterConfiguration
                {
                    Id = Globals.TestAdapterId,
                    IpAddress = "",
                    Name = "Тестовый",
                    Type = AdapterType.Test
                });

                for (int i = 0; i < 5; i++)
                {
                    ctx.DataPoints.Add(new DataPointConfiguration
                    {
                        Name = $"Variable{i}",
                        Type = TypeCode.Double,
                        AdapterId = Globals.TestAdapterId,
                        Mapping = $"mappingparams({i})"
                    });
                }
                for (int i = 0; i < 2; i++)
                {
                    ctx.DataPoints.Add(new DataPointConfiguration
                    {
                        Name = $"Command{i}",
                        Type = TypeCode.Boolean,
                        Source = DataSource.Control,
                        AdapterId = Globals.TestAdapterId,
                        Mapping = $"mappingparams({i})"
                    });
                }

                ctx.SaveChanges();
            }
        }
    }
}

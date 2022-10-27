using Autonoma.Communication.Modbus;
using Autonoma.Communication.Test.Client;
using Autonoma.Configuration;
using Autonoma.Core;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using System;
using System.Linq;

namespace Autonoma.ConfigurationSeeder
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
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            CheckTestAdapters();
            CheckTestDataPoints();

            CheckTestModel();
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
                AdapterType = idleAdapterType,
            });

            var testAdapterType = new AdapterType
            {
                Id = Globals.TestAdapterTypeId,
                Name = "Тестовый",
                AssemblyQualifiedAdapterTypeName = typeof(TestClient).AssemblyQualifiedName,
                AssemblyQualifiedConfigurationTypeName = typeof(TestClientConfiguration).AssemblyQualifiedName,
            };
            _context.AdapterTypes.Add(testAdapterType);
            _context.Adapters.Add(new AdapterConfiguration
            {
                Id = Globals.TestAdapterId,
                IpAddress = "",
                Name = "Тестовый",
                AdapterType = testAdapterType,
            });

            var modbusAdapterType = new AdapterType
            {
                Id = 3,
                Name = "Modbus",
                AssemblyQualifiedAdapterTypeName = typeof(ModbusClient).AssemblyQualifiedName,
                AssemblyQualifiedConfigurationTypeName = typeof(ModbusClientConfiguration).AssemblyQualifiedName,
            };
            _context.AdapterTypes.Add(modbusAdapterType);
            _context.Adapters.Add(new AdapterConfiguration
            {
                Id = 31,
                IpAddress = "",
                Name = "Modbus #1",
                AdapterType = modbusAdapterType,
                Configuration = ""
            });

            _context.SaveChanges();
        }

        // 40 тегов, 3 команды
        private void CheckTestDataPoints()
        {
            if (_context.DataPoints.Any())
                return;

            for (int i = 1; i <= 20; i++)
            {
                _context.DataPoints.Add(new DataPointConfiguration
                {
                    Id = i,
                    Name = $"Variable{i}",
                    Type = TypeCode.Double,
                    AdapterId = Globals.TestAdapterId,
                    Mapping = $"mappingparams({i})"
                });
            }

            for (int i = 21; i <= 40; i++)
            {
                _context.DataPoints.Add(new DataPointConfiguration
                {
                    Id = i,
                    Name = $"Variable{i}",
                    Type = TypeCode.Double,
                    AdapterId = 31,
                    Mapping = $"mappingparams({i})"
                });
            }

            for (int i = 1001; i <= 1010; i++)
            {
                _context.DataPoints.Add(new DataPointConfiguration
                {
                    Id = i,
                    Name = $"Command{i}",
                    Type = TypeCode.Boolean,
                    Source = DataSource.Control,
                    AdapterId = Globals.TestAdapterId,
                    Mapping = $"mappingparams({i})"
                });
            }

            _context.SaveChanges();
        }

        // 40 элементов
        private void CheckTestModel()
        {
            if (_context.ModelTemplates.Any())
                return;
            if (_context.ModelElements.Any())
                return;

            var dataObjectTemplate = new ModelElementTemplateConfiguration
            {
                Name = "DataObject",
                Id = Globals.DataObjectTemplateId,
            };

            var measuredValueIntTemplate = new ModelElementTemplateConfiguration
            {
                Name = "MeasuredValueDouble",
                Id = Globals.MeasuredValueIntModelTemplateId,
                BaseTemplate = dataObjectTemplate
            };
            measuredValueIntTemplate.AddAttribute("Value", TypeCode.Int32);
            measuredValueIntTemplate.AddAttribute("Timestamp", TypeCode.DateTime);

            var measuredValueIntQualifiedTemplate = new ModelElementTemplateConfiguration
            {
                Name = "MeasuredValueIntWithQuality",
                Id = Globals.MeasuredValueIntWithQualityModelTemplateId,
                BaseTemplate = measuredValueIntTemplate,
            };
            measuredValueIntQualifiedTemplate.AddAttribute("Quality", TypeCode.Int32);
            measuredValueIntQualifiedTemplate.AddAttribute("High", TypeCode.Int32, ModelAttributeType.Setting);
            measuredValueIntQualifiedTemplate.AddAttribute("Low", TypeCode.Int32, ModelAttributeType.Setting);
            measuredValueIntQualifiedTemplate.AddAttribute("HighHigh", TypeCode.Int32, ModelAttributeType.Setting);
            measuredValueIntQualifiedTemplate.AddAttribute("LowLow", TypeCode.Int32, ModelAttributeType.Setting);
            measuredValueIntQualifiedTemplate.AddAttribute("Range", TypeCode.Int32);

            var measuredValueDoubleTemplate = new ModelElementTemplateConfiguration
            {
                Name = "MeasuredValueDouble",
                Id = Globals.MeasuredValueDoubleModelTemplateId,
                BaseTemplate = dataObjectTemplate
            };
            measuredValueDoubleTemplate.AddAttribute("Value", TypeCode.Double);
            measuredValueDoubleTemplate.AddAttribute("Timestamp", TypeCode.DateTime);

            var measuredValueDoubleQualifiedTemplate = new ModelElementTemplateConfiguration
            {
                Name = "MeasuredValueDoubleWithQuality",
                Id = Globals.MeasuredValueDoubleWithQualityModelTemplateId,
                BaseTemplate = measuredValueDoubleTemplate,
            };
            measuredValueDoubleQualifiedTemplate.AddAttribute("Quality", TypeCode.Int32);
            measuredValueDoubleQualifiedTemplate.AddAttribute("High", TypeCode.Double, ModelAttributeType.Setting);
            measuredValueDoubleQualifiedTemplate.AddAttribute("Low", TypeCode.Double, ModelAttributeType.Setting);
            measuredValueDoubleQualifiedTemplate.AddAttribute("HighHigh", TypeCode.Double, ModelAttributeType.Setting);
            measuredValueDoubleQualifiedTemplate.AddAttribute("LowLow", TypeCode.Double, ModelAttributeType.Setting);
            measuredValueDoubleQualifiedTemplate.AddAttribute("Range", TypeCode.Int32);

            _context.ModelTemplates.Add(dataObjectTemplate);
            _context.ModelTemplates.Add(measuredValueIntTemplate);
            _context.ModelTemplates.Add(measuredValueIntQualifiedTemplate);
            _context.ModelTemplates.Add(measuredValueDoubleTemplate);
            _context.ModelTemplates.Add(measuredValueDoubleQualifiedTemplate);

            _context.SaveChanges();

            if (_context.ModelElements.Any())
                return;

            for (int j = 0; j <= 3; j++)
            {
                var groupElement = new ModelElementConfiguration
                {
                    Id = j * 1000,
                    Name = $"ElementOwner_{j}",
                    Template = measuredValueDoubleTemplate,
                };
                _context.ModelElements.Add(groupElement);

                for (int i = 1; i <= 5; i++)
                {
                    var element = new ModelElementConfiguration
                    {
                        Id = j * 10 + i,
                        Name = $"Element{j * 10 + i}",
                        Template = measuredValueDoubleTemplate,
                        ParentElement = groupElement,
                        DataPointId = j * 10 + i,
                    };
                    _context.ModelElements.Add(element);
                }

                for (int i = 6; i <= 10; i++)
                {
                    var element = new ModelElementConfiguration
                    {
                        Id = j * 10 + i,
                        Name = $"Element{j * 10 + i}",
                        Template = measuredValueDoubleQualifiedTemplate,
                        ParentElement = groupElement,
                        DataPointId = j * 10 + i,
                    };
                    _context.ModelElements.Add(element);
                }
            }

            _context.SaveChanges();
        }
    }
}

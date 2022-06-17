using Autonoma.Communication.Modbus;
using Autonoma.Communication.Test.Client;
using Autonoma.Configuration;
using Autonoma.Core;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using System;
using System.Collections.Generic;
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

        private void CheckTestDataPoints()
        {
            if (_context.DataPoints.Any())
                return;

            for (int i = 0; i < 50; i++)
            {
                _context.DataPoints.Add(new DataPointConfiguration
                {
                    Name = $"Variable{i}",
                    Type = TypeCode.Double,
                    AdapterId = Globals.TestAdapterId,
                    Mapping = $"mappingparams({i})"
                });
                _context.DataPoints.Add(new DataPointConfiguration
                {
                    Name = $"Variable{i}",
                    Type = TypeCode.Double,
                    AdapterId = 31,
                    Mapping = $"mappingparams({i})"
                });
            }
            for (int i = 0; i < 10; i++)
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

        private void CheckTestModel()
        {
            if (_context.ModelTemplates.Any())
                return;
            if (_context.ModelElements.Any())
                return;

            var baseTemplate = new ModelElementTemplateConfiguration
            {
                Name = $"BaseTemplate"
            };
            baseTemplate.Attributes.Add(new ModelAttributeTemplateConfiguration
            {
                Name = "AttributeInt",
                Type = TypeCode.Int32,
                Template = baseTemplate
            });
            baseTemplate.Attributes.Add(new ModelAttributeTemplateConfiguration
            {
                Name = "AttributeString",
                Type = TypeCode.String,
                Template = baseTemplate
            });

            var derivedTemplate = new ModelElementTemplateConfiguration
            {
                Name = $"DerivedTemplate",
                BaseTemplate = baseTemplate,
            };
            derivedTemplate.Attributes.Add(new ModelAttributeTemplateConfiguration
            {
                Name = "AttributeInt2",
                Type = TypeCode.Int32,
                Template = derivedTemplate
            });
            derivedTemplate.Attributes.Add(new ModelAttributeTemplateConfiguration
            {
                Name = "AttributeString2",
                Type = TypeCode.String,
                Template = derivedTemplate
            });

            _context.ModelTemplates.Add(baseTemplate);
            _context.ModelTemplates.Add(derivedTemplate);

            _context.SaveChanges();

            if (_context.ModelElements.Any())
                return;

            for (int j = 0; j < 4; j++)
            {
                var groupElement = new ModelElementConfiguration
                {
                    Name = $"Element{j}",
                    Template = baseTemplate,
                };
                _context.ModelElements.Add(groupElement);

                for (int i = 0; i < 10; i++)
                {
                    var element = new ModelElementConfiguration
                    {
                        Name = $"Element{j}-{i}",
                        Template = baseTemplate,
                        ParentElement = groupElement,
                    };
                    _context.ModelElements.Add(element);
                }

                for (int i = 0; i < 10; i++)
                {
                    var element = new ModelElementConfiguration
                    {
                        Name = $"Element{j}-{i}",
                        Template = derivedTemplate,
                        ParentElement = groupElement,
                    };
                    _context.ModelElements.Add(element);
                }
            }

            _context.SaveChanges();
        }
    }
}

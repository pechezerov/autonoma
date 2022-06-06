using Microsoft.EntityFrameworkCore;
using PrettyScreen.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrettyScreen.Configuration.Repositories
{
    public class ConfigurationRepository
        : IDataStore<AdapterConfiguration>, IDataStore<DataPointConfiguration>
    {
        public IEnumerable<AdapterConfiguration> GetAdapters()
        {
            using (var ctx = new ConfigurationContext())
            {
                return ctx.Adapters
                    .Include(a => a.DataPoints)
                    .ToList();
            }
        }

        public IEnumerable<DataPointConfiguration> GetDataPoints()
        {
            using (var ctx = new ConfigurationContext())
            {
                return ctx.DataPoints.ToList();
            }
        }

        IEnumerable<DataPointConfiguration> IDataStore<DataPointConfiguration>.Items => GetDataPoints();

        IEnumerable<AdapterConfiguration> IDataStore<AdapterConfiguration>.Items => GetAdapters();

        public void AddItem(AdapterConfiguration item)
        {
            using (var ctx = new ConfigurationContext())
            {
                if (!ctx.Adapters.Any(a => a.Id == item.Id))
                {
                    ctx.Adapters.Add(item);
                }
                ctx.SaveChanges();
            }
        }

        public void AddItem(DataPointConfiguration item)
        {
            using (var ctx = new ConfigurationContext())
            {
                if (!ctx.DataPoints.Any(a => a.Id == item.Id))
                {
                    ctx.DataPoints.Add(item);
                }
                ctx.SaveChanges();
            }
        }

        public void DeleteAdapter(Guid id)
        {
            using (var ctx = new ConfigurationContext())
            {
                var target = ctx.Adapters.Find(id);
                if (target != null)
                {
                    ctx.Adapters.Remove(target);
                    ctx.SaveChanges();
                }
            }
        }

        public void DeleteDataPoint(Guid id)
        {
            using (var ctx = new ConfigurationContext())
            {
                var target = ctx.DataPoints.Find(id);
                if (target != null)
                {
                    ctx.DataPoints.Remove(target);
                    ctx.SaveChanges();
                }
            }
        }

        public AdapterConfiguration GetAdapter(Guid id)
        {
            using (var ctx = new ConfigurationContext())
            {
                return ctx.Adapters.Find(id);
            }
        }

        public DataPointConfiguration GetDataPoint(Guid id)
        {
            using (var ctx = new ConfigurationContext())
            {
                return ctx.DataPoints.Find(id);
            }
        }

        public void UpdateItem(AdapterConfiguration item)
        {
            using (var ctx = new ConfigurationContext())
            {
                if (ctx.Adapters.Any(a => a.Id == item.Id))
                {
                    ctx.Adapters.Update(item);
                }
                else
                {
                    ctx.Adapters.Add(item);
                }
                ctx.SaveChanges();
            }
        }

        public void UpdateItem(DataPointConfiguration item)
        {
            using (var ctx = new ConfigurationContext())
            {
                if (ctx.DataPoints.Any(a => a.Id == item.Id))
                {
                    ctx.DataPoints.Update(item);
                }
                else
                {
                    ctx.DataPoints.Add(item);
                }
                ctx.SaveChanges();
            }
        }

        void IDataStore<DataPointConfiguration>.DeleteItem(Guid id)
        {
            DeleteDataPoint(id);
        }

        void IDataStore<AdapterConfiguration>.DeleteItem(Guid id)
        {
            DeleteAdapter(id);
        }

        DataPointConfiguration IDataStore<DataPointConfiguration>.GetItem(Guid id)
        {
            return GetDataPoint(id);
        }

        AdapterConfiguration IDataStore<AdapterConfiguration>.GetItem(Guid id)
        {
            return GetAdapter(id);
        }
    }
}

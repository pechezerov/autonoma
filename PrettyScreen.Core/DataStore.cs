using PrettyScreen.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrettyScreen.Core
{
    public class DataStore<T> : IDataStore<T> where T: IUnique
    {
        protected List<T> items = new List<T>();

        public DataStore()
        {
        }

        public void AddItem(T item)
        {
            items.Add(item);
        }

        public void UpdateItem(T item)
        {
            var oldItem = items.Where((T arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);
        }

        public void DeleteItem(Guid id)
        {
            var oldItem = items.Where((T arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);
        }

        public T GetItem(Guid id)
        {
            var result = items.Where((T arg) => arg.Id == id).FirstOrDefault();
            return result;
        }

        public IEnumerable<T> Items => items;
    }
}
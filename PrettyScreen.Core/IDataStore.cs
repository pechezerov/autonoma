using System;
using System.Collections.Generic;

namespace PrettyScreen.Core
{
    public interface IDataStore<T>
    {
        void AddItem(T item);
        void UpdateItem(T item);
        void DeleteItem(Guid id);
        T GetItem(Guid id);

        IEnumerable<T> Items { get; }
    }
}

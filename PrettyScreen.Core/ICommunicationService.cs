using System;
using System.Collections.Generic;

namespace PrettyScreen.Core
{
    public interface ICommunicationService : IDisposable
    {
        IEnumerable<IDataAdapter> Adapters { get; }
    }
}
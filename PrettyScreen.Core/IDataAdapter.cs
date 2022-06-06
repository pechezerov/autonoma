using System;

namespace PrettyScreen.Core
{
    public interface IDataAdapter : IUnique, IDisposable
    {
        void Start();
        void Stop();
        WorkState State { get; }
    }
}
using Microsoft.Extensions.Hosting;
using System;

namespace Autonoma.Domain.Abstractions
{
    public interface IDataAdapter : IHostedService, IUnique, IDisposable
    {
        WorkState State { get; }
    }
}
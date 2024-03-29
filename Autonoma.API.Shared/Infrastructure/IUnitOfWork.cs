﻿using Autonoma.Configuration.Repositories.Abstractions;
using System.Threading.Tasks;

namespace Autonoma.API.Infrastructure
{
    public interface IUnitOfWork
    {
        public IAdapterRepository AdapterRepository { get; }
        public IDataPointRepository DataPointRepository { get; }
        public IModelElementRepository ModelRepository { get; }
        public IModelElementTemplateRepository ModelTemplateRepository { get; }

        int Commit();
        Task<int> CommitAsync();
    }
}
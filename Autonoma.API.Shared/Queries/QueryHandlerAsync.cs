using Autonoma.API.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Autonoma.API.Queries
{
    public abstract class QueryHandlerAsync<TQuery, TResult> : IQueryHandlerAsync<TQuery, TResult>
            where TQuery : IQuery
            where TResult : IQueryResult
    {
        protected readonly IUnitOfWork _uow;

        public QueryHandlerAsync(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public abstract Task<TResult> ExecuteAsync(TQuery query);
    }
}

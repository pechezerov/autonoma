using Autonoma.Configuration;
using System;

namespace Autonoma.API.Queries
{
    public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery
        where TResult : IQueryResult
    {
        protected readonly ConfigurationContext _context;

        public QueryHandler(ConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public abstract TResult Execute(TQuery query);
    }
}

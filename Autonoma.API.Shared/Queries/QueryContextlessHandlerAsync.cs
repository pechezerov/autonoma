using System.Threading.Tasks;

namespace Autonoma.API.Queries
{
    public abstract class QueryContextlessHandlerAsync<TQuery, TResult> : IQueryHandlerAsync<TQuery, TResult>
            where TQuery : IQuery
            where TResult : IQueryResult
    {
        public QueryContextlessHandlerAsync()
        {
        }

        public abstract Task<TResult> ExecuteAsync(TQuery query);
    }
}

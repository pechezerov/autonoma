using System.Threading.Tasks;

namespace Autonoma.API.Queries
{
    public interface IQueryHandlerAsync<TQuery, TResult>
            where TQuery : IQuery
            where TResult : IQueryResult
    {
        Task<TResult> ExecuteAsync(TQuery query);

    }
}

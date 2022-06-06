namespace Autonoma.API.Queries
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery
        where TResult : IQueryResult
    {
        TResult Execute(TQuery query);

    }
}
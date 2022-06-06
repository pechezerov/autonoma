using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.API.Queries;
using Autonoma.Communication.Hosting;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.DataPoint
{
    public class DataPointUpdateListQueryHandler : QueryHandlerAsync<DataPointUpdateListQuery, DataPointUpdateQueryResult>
    {
        private IDataPointService _dataPointService;

        public DataPointUpdateListQueryHandler(IUnitOfWork uow, IDataPointService dataPointService)
             : base(uow)
        {
            _dataPointService = dataPointService;
        }

        public override async Task<DataPointUpdateQueryResult> ExecuteAsync(DataPointUpdateListQuery query)
        {
            await _dataPointService.UpdateDataPoints(query.Data);
            return new DataPointUpdateQueryResult();
        }
    }
}
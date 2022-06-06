using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.API.Queries;
using Autonoma.Communication.Hosting;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.DataPoint
{
    public class DataPointUpdateQueryHandler : QueryHandlerAsync<DataPointUpdateQuery, DataPointUpdateQueryResult>
    {
        private IDataPointService _dataPointService;

        public DataPointUpdateQueryHandler(IUnitOfWork uow, IDataPointService dataPointService)
             : base(uow)
        {
            _dataPointService = dataPointService;
        }

        public override async Task<DataPointUpdateQueryResult> ExecuteAsync(DataPointUpdateQuery query)
        {
            await _dataPointService.UpdateDataPoint(query.DataPointId, query.Data);
            return new DataPointUpdateQueryResult();
        }
    }
}
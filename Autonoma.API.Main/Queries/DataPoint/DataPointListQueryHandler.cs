using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.API.Queries;
using Autonoma.Communication.Hosting;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.DataPoint
{
    public class DataPointListQueryHandler : QueryHandlerAsync<DataPointListQuery, DataPointListQueryResult>
    {
        private readonly IDataPointService _dataPointService;

        public DataPointListQueryHandler(IUnitOfWork uow, IDataPointService dataPointService)
             : base(uow)
        {
            _dataPointService = dataPointService;
        }

        public override async Task<DataPointListQueryResult> ExecuteAsync(DataPointListQuery query)
        {
            return new DataPointListQueryResult
            {
                DataPoints = await _dataPointService.GetDataPointValues(query.Ids)
            };
        }
    }
}

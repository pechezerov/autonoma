using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.API.Queries;
using Autonoma.Communication.Hosting;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.DataPoint
{
    public class DataPointByIdQueryHandler : QueryHandlerAsync<DataPointByIdQuery, DataPointByIdQueryResult>
    {
        private readonly IDataPointService _dataPointService;

        public DataPointByIdQueryHandler(IUnitOfWork uow, IDataPointService dataPointService)
             : base(uow)
        {
            _dataPointService = dataPointService;
        }

        public override Task<DataPointByIdQueryResult> ExecuteAsync(DataPointByIdQuery query)
        {
            return Task.Run(async () => 
            new DataPointByIdQueryResult
            {
                DataPoint = await _dataPointService.GetDataPointValue(query.Id)
            });
        }
    }
}

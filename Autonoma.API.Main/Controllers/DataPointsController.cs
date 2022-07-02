using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.API.Main.Queries.DataPoint;
using Autonoma.API.Queries;
using Autonoma.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataPointsController : ControllerBase
    {
        private readonly IQueryHandlerAsync<DataPointByIdQuery, DataPointByIdQueryResult> _dataPointByIdHandler;
        private readonly IQueryHandlerAsync<DataPointListQuery, DataPointListQueryResult> _dataPointListHandler;
        private readonly IQueryHandlerAsync<DataPointUpdateQuery, DataPointUpdateQueryResult> _dataPointUpdateHandler;
        private readonly IQueryHandlerAsync<DataPointUpdateListQuery, DataPointUpdateQueryResult> _dataPointUpdateListHandler;

        public DataPointsController(
            IQueryHandlerAsync<DataPointByIdQuery, DataPointByIdQueryResult> dataPointByIdHandler,
            IQueryHandlerAsync<DataPointListQuery, DataPointListQueryResult> dataPointListHandler,
            IQueryHandlerAsync<DataPointUpdateQuery, DataPointUpdateQueryResult> dataPointUpdateHandler,
            IQueryHandlerAsync<DataPointUpdateListQuery, DataPointUpdateQueryResult> dataPointUpdateListHandler)
        {
            _dataPointByIdHandler = dataPointByIdHandler;
            _dataPointListHandler = dataPointListHandler;
            _dataPointUpdateHandler = dataPointUpdateHandler;
            _dataPointUpdateListHandler = dataPointUpdateListHandler;
        }

        // GET api/v1/[controller]/123
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(DataPointByIdQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DataPointById(int id)
        {
            var query = new DataPointByIdQuery(id);
            return Ok(await _dataPointByIdHandler.ExecuteAsync(query));
        }

        // GET api/v1/[controller]/1,2,3
        [HttpGet]
        [Route("{ids}")]
        [ProducesResponseType(typeof(DataPointListQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DataPointList(string ids)
        {
            var query = new DataPointListQuery(ids);
            return Ok(await _dataPointListHandler.ExecuteAsync(query));
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(DataPointUpdateQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDataPoint(int id, DataValue data)
        {
            return Ok(await _dataPointUpdateHandler.ExecuteAsync(new DataPointUpdateQuery(id, data)));
        }

        [HttpPut]
        [Route("updateList")]
        [ProducesResponseType(typeof(DataPointUpdateQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateDataPoints([FromBody] DataPointUpdateListQuery query)
        {
            return Ok(await _dataPointUpdateListHandler.ExecuteAsync(query));
        }
    }
}

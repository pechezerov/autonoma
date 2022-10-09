using Microsoft.AspNetCore.Mvc;
using Autonoma.API.Queries;
using System.Net;
using System.Threading.Tasks;
using Autonoma.API.Main.Commands.DataPoint;
using Autonoma.API.Commands;
using Autonoma.API.Main.Contracts.DataPoint;

namespace Autonoma.API.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataPointsConfigurationController : ControllerBase
    {
        private readonly IQueryHandlerAsync<DataPointConfigurationListQuery, DataPointConfigurationListQueryResult> _dataPointListHandler;
        private readonly IQueryHandlerAsync<DataPointConfigurationByIdQuery, DataPointConfigurationByIdQueryResult> _dataPointByIdHandler;
        private readonly ICommandHandlerAsync<DataPointCreateCommand> _dataPointCreateCommandHandler;
        private readonly ICommandHandlerAsync<DataPointUpdateCommand> _dataPointUpdateCommandHandler;
        private readonly ICommandHandlerAsync<DataPointDeleteCommand> _dataPointDeleteCommandHandler;

        public DataPointsConfigurationController(
            IQueryHandlerAsync<DataPointConfigurationListQuery, DataPointConfigurationListQueryResult> dataPointListHandler,
            IQueryHandlerAsync<DataPointConfigurationByIdQuery, DataPointConfigurationByIdQueryResult> dataPointByIdHandler,
            ICommandHandlerAsync<DataPointCreateCommand> dataPointCreateCommandHandler,
            ICommandHandlerAsync<DataPointUpdateCommand> dataPointUpdateCommandHandler,
            ICommandHandlerAsync<DataPointDeleteCommand> dataPointDeleteCommandHandler)
        {
            _dataPointListHandler = dataPointListHandler;
            _dataPointByIdHandler = dataPointByIdHandler;
            _dataPointCreateCommandHandler = dataPointCreateCommandHandler;
            _dataPointUpdateCommandHandler = dataPointUpdateCommandHandler;
            _dataPointDeleteCommandHandler = dataPointDeleteCommandHandler;
        }

        // GET api/v1/[controller]/123
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(DataPointConfigurationByIdQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DataPointById(int id)
        {
            var query = new DataPointConfigurationByIdQuery(id);
            return Ok(await _dataPointByIdHandler.ExecuteAsync(query));
        }

        // POST api/v1/[controller]/list
        [HttpPost]
        [Route("list")]
        [ProducesResponseType(typeof(DataPointConfigurationListQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DataPointList([FromBody] DataPointConfigurationListQuery query)
        {
            return Ok(await _dataPointListHandler.ExecuteAsync(query));
        }

        // POST api/v1/[controller]/create
        [HttpPost("create")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateDataPoint(DataPointConfigurationItem datapoint)
        {
            var command = new DataPointCreateCommand
            {
                DataPoint = datapoint
            };
            await _dataPointCreateCommandHandler.ExecuteAsync(command);
            return CreatedAtAction(
                nameof(DataPointById),
                new { id = command.CreatedId },
                command.CreatedId);
        }

        // PUT api/v1/[controller]/delete/123
        [HttpDelete]
        [Route("delete/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteDataPoint(int id)
        {
            var command = new DataPointDeleteCommand
            {
                Id = id
            };
            await _dataPointDeleteCommandHandler.ExecuteAsync(command);
            return NoContent();
        }

        // PUT api/v1/[controller]/update
        [HttpPut("update")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateDataPoint(DataPointConfigurationItem dataPoint)
        {
            var command = new DataPointUpdateCommand(dataPoint);
            await _dataPointUpdateCommandHandler.ExecuteAsync(command);
            return NoContent();
        }

    }
}

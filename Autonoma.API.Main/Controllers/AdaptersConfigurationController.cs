using Autonoma.API.Commands;
using Autonoma.API.Main.Commands.Adapter;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdaptersConfigurationController : ControllerBase
    {
        private readonly IQueryHandlerAsync<AdapterConfigurationListQuery, AdapterConfigurationListQueryResult> _adapterListHandler;
        private readonly IQueryHandlerAsync<AdapterConfigurationByIdQuery, AdapterConfigurationByIdQueryResult> _adapterByIdHandler;
        private readonly ICommandHandlerAsync<AdapterCreateCommand> _adapterCreateCommandHandler;
        private readonly ICommandHandlerAsync<AdapterUpdateCommand> _adapterUpdateCommandHandler;
        private readonly ICommandHandlerAsync<AdapterDeleteCommand> _adapterDeleteCommandHandler;

        public AdaptersConfigurationController(
            IQueryHandlerAsync<AdapterConfigurationListQuery, AdapterConfigurationListQueryResult> adapterListHandler,
            IQueryHandlerAsync<AdapterConfigurationByIdQuery, AdapterConfigurationByIdQueryResult> adapterByIdHandler,
            ICommandHandlerAsync<AdapterCreateCommand> adapterCreateCommandHandler,
            ICommandHandlerAsync<AdapterUpdateCommand> adapterUpdateCommandHandler,
            ICommandHandlerAsync<AdapterDeleteCommand> adapterDeleteCommandHandler)
        {
            _adapterListHandler = adapterListHandler;
            _adapterByIdHandler = adapterByIdHandler;
            _adapterCreateCommandHandler = adapterCreateCommandHandler;
            _adapterUpdateCommandHandler = adapterUpdateCommandHandler;
            _adapterDeleteCommandHandler = adapterDeleteCommandHandler;
        }

        // GET api/v1/[controller]/list[?pageSize=30&pageIndex=10]
        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(AdapterConfigurationListQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdapterList([FromQuery] AdapterConfigurationListQuery query)
        {
            return Ok(await _adapterListHandler.ExecuteAsync(query));
        }


        // GET api/v1/[controller]/123
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(AdapterConfigurationByIdQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdapterById(int id)
        {
            var query = new AdapterConfigurationByIdQuery(id);
            return Ok(await _adapterByIdHandler.ExecuteAsync(query));
        }

        // POST api/v1/[controller]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateAdapter(AdapterConfigurationItem adapter)
        {
            var command = new AdapterCreateCommand
            {
                Adapter = adapter
            };
            await _adapterCreateCommandHandler.ExecuteAsync(command);
            return CreatedAtAction(
                nameof(AdapterById),
                new { id = command.CreatedId },
                command.CreatedId);
        }

        // POST api/v1/[controller]
        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAdapter(int id)
        {
            var command = new AdapterDeleteCommand
            {
                Id = id
            };
            await _adapterDeleteCommandHandler.ExecuteAsync(command);
            return NoContent();
        }

        // PUT api/v1/[controller]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateAdapter(AdapterConfigurationItem adapter)
        {
            var command = new AdapterUpdateCommand
            {
                Adapter = adapter
            };
            await _adapterUpdateCommandHandler.ExecuteAsync(command);
            return NoContent();
        }
    }
}

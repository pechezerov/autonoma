using Autonoma.API.Commands;
using Autonoma.API.Main.Commands.Adapter;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Queries;
using Autonoma.Configuration;
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

        public AdaptersConfigurationController(ConfigurationContext ctx,
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

        // GET api/v1/[controller]/123
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(AdapterConfigurationByIdQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdapterConfigurationById(int id)
        {
            var query = new AdapterConfigurationByIdQuery(id);
            return Ok(await _adapterByIdHandler.ExecuteAsync(query));
        }

        // POST api/v1/[controller]/list
        [HttpPost]
        [Route("list")]
        [ProducesResponseType(typeof(AdapterConfigurationListQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdapterConfiguration([FromBody] AdapterConfigurationListQuery query)
        {
            return Ok(await _adapterListHandler.ExecuteAsync(query));
        }

        // POST api/v1/[controller]/create
        [HttpPost("create")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateAdapterConfiguration(AdapterConfigurationItem adapter)
        {
            var command = new AdapterCreateCommand
            {
                Adapter = adapter
            };
            await _adapterCreateCommandHandler.ExecuteAsync(command);
            return CreatedAtAction(
                nameof(AdapterConfigurationById),
                new { id = command.CreatedId },
                command.CreatedId);
        }

        // POST api/v1/[controller]/delete/123
        [HttpDelete]
        [Route("delete/{id:int}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAdapterConfiguration(int id)
        {
            var command = new AdapterDeleteCommand
            {
                Id = id
            };
            await _adapterDeleteCommandHandler.ExecuteAsync(command);
            return NoContent();
        }

        // PUT api/v1/[controller]/update
        [HttpPut("update")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateAdapterConfiguration(AdapterConfigurationItem adapter)
        {
            var command = new AdapterUpdateCommand(adapter);
            await _adapterUpdateCommandHandler.ExecuteAsync(command);
            return NoContent();
        }
    }
}

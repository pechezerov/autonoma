using Autonoma.API.Commands;
using Autonoma.API.Main.Commands.Adapter;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Queries;
using Autonoma.Core.Util;
using Autonoma.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdaptersController : ControllerBase
    {
        private readonly IQueryHandlerAsync<AdapterListQuery, AdapterListQueryResult> _adapterListHandler;
        private readonly IQueryHandlerAsync<AdapterByIdQuery, AdapterByIdQueryResult> _adapterByIdHandler;
        private readonly ICommandHandlerAsync<AdapterStartCommand> _adapterStartCommandHandler;
        private readonly ICommandHandlerAsync<AdapterStopCommand> _adapterStopCommandHandler;

        public AdaptersController(
            IQueryHandlerAsync<AdapterListQuery, AdapterListQueryResult> adapterListHandler,
            IQueryHandlerAsync<AdapterByIdQuery, AdapterByIdQueryResult> adapterByIdHandler,
            ICommandHandlerAsync<AdapterStartCommand> adapterStartCommandHandler,
            ICommandHandlerAsync<AdapterStopCommand> adapterStopCommandHandler)
        {
            _adapterListHandler = adapterListHandler;
            _adapterByIdHandler = adapterByIdHandler;
            _adapterStartCommandHandler = adapterStartCommandHandler;
            _adapterStopCommandHandler = adapterStopCommandHandler;
        }

        // GET api/v1/[controller]/123
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(AdapterByIdQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdapterById(int id)
        {
            var query = new AdapterByIdQuery(id);
            return Ok(await _adapterByIdHandler.ExecuteAsync(query));
        }

        // POST api/v1/[controller]/list
        [HttpPost]
        [Route("list")]
        [ProducesResponseType(typeof(AdapterListQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AdapterList([FromBody] AdapterListQuery query)
        {
            return Ok(await _adapterListHandler.ExecuteAsync(query));
        }

        // POST api/v1/[controller]/123/start
        [HttpPost]
        [Route("{id:int}/start")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> StartAdapter()
        {
            await _adapterStartCommandHandler.ExecuteAsync(
                new AdapterStartCommand());
            return NoContent();
        }

        // POST api/v1/[controller]/123/stop
        [HttpPost]
        [Route("{id:int}/stop")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> StopAdapter()
        {
            await _adapterStopCommandHandler.ExecuteAsync(
                new AdapterStopCommand());
            return NoContent();
        }

        [HttpGet("workstate/list")]
        [ProducesResponseType(typeof(EnumValue[]), (int)HttpStatusCode.OK)]
        public IActionResult GetStateList()
        {
            var result = EnumHelper.GetEnumValueList<WorkState>();
            return Ok(result);
        }
    }
}

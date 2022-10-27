using Autonoma.API.Commands;
using Autonoma.API.Main.Commands.Administration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministrationController : ControllerBase
    {
        private readonly ICommandHandlerAsync<SystemUpdateCommand> _systemUpdateCommandHandler;
        private readonly ICommandHandlerAsync<SystemResetCommand> _systemResetCommandHandler;

        public AdministrationController(
            ICommandHandlerAsync<SystemUpdateCommand> systemUpdateCommandHandler,
            ICommandHandlerAsync<SystemResetCommand> systemResetCommandHandler)
        {
            _systemUpdateCommandHandler = systemUpdateCommandHandler;
            _systemResetCommandHandler = systemResetCommandHandler;
        }

        // POST api/v1/[controller]/updateSystem
        [HttpPost("updateSystem")]
        public async Task<IActionResult> UpdateSystem()
        {
            var command = new SystemUpdateCommand();
            await _systemUpdateCommandHandler.ExecuteAsync(command);
            return Ok();
        }

        // POST api/v1/[controller]/resetSystem
        [HttpPost("resetSystem")]
        public async Task<IActionResult> ResetSystem()
        {
            var command = new SystemResetCommand();
            await _systemResetCommandHandler.ExecuteAsync(command);
            return Ok();
        }
    }
}

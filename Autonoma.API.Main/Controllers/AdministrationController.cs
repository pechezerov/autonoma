using Autonoma.API.Commands;
using Autonoma.API.Main.Commands.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministrationController : ControllerBase
    {
        private readonly ICommandHandlerAsync<SystemUpdateCommand> _systemUpdateCommandHandler;

        public AdministrationController(ICommandHandlerAsync<SystemUpdateCommand> systemUpdateCommandHandler)
        {
            _systemUpdateCommandHandler = systemUpdateCommandHandler;
        }

        // POST api/v1/[controller]/updateSystem
        [HttpPost]
        public async Task<IActionResult> UpdateSystem()
        {
            var command = new SystemUpdateCommand();
            await _systemUpdateCommandHandler.ExecuteAsync(command);
            return Ok();
        }
    }
}

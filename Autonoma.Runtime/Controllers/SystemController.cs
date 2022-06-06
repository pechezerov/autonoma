using AutoMapper;
using Autonoma.ProcessManagement;
using Autonoma.ProcessManagement.Abstractions;
using Autonoma.Runtime.Queries.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Autonoma.Runtime.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly IProcessManager _processManager;
        private readonly IMapper _mapper;

        public SystemController(IProcessManager manager, IMapper mapper)
        {
            _processManager = manager;
            _mapper = mapper;
        }

        // GET api/v1/[controller]/processList[?pageSize=30&pageIndex=10]
        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(ProcessListQueryResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetProcessList()
        {
            var processes = await _processManager.GetProcessList();
            return Ok(new ProcessListQueryResult(processes
                .Select(p => _mapper.Map<ProcessBase, ProcessItem>(p))));
        }
    }
}

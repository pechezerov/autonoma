using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using Autonoma.Configuration;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.Administration
{
    public class SystemResetCommand : Command
    {
    }

    public class SystemResetCommandHandler : CommandHandlerAsync<SystemResetCommand>
    {
        private readonly IConfiguration _configuration;
        private readonly ConfigurationContext _context;

        public SystemResetCommandHandler(IUnitOfWork uow, IConfiguration configuration, ConfigurationContext context)
            : base(uow)
        {
            _configuration = configuration;
            _context = context;
        }

        public override async Task ExecuteAsync(SystemResetCommand command)
        {
            var initializer = new DatabaseInitializer(_configuration, _context);
            initializer.Start();
            await Task.CompletedTask;
        }
    }
}
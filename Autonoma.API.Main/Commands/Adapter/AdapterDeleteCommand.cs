﻿using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Infrastructure;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.Adapter
{
    public class AdapterDeleteCommand : Command
    {
        public int Id { get; internal set; }
    }

    public class AdapterDeleteCommandHandler : CommandHandlerAsync<AdapterDeleteCommand>
    {
        public AdapterDeleteCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(AdapterDeleteCommand command)
        {
            await Task.CompletedTask;
        }
    }
}
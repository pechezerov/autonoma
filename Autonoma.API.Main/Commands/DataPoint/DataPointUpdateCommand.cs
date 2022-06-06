using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.API.Main.Infrastructure;
using Autonoma.API.Main.Queries.DataPoint;
using Autonoma.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.DataPoint
{
    public class DataPointUpdateCommand : Command
    {
        public DataPointConfigurationItem DataPoint { get; set; }
    }

    public class DataPointUpdateCommandHandler : CommandHandlerAsync<DataPointUpdateCommand>
    {
        public DataPointUpdateCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(DataPointUpdateCommand command)
        {
            var dataPointInfo = command.DataPoint;

            var dataPoint = new DataPointConfiguration
            {
                Id = dataPointInfo.Id,
                AdapterId = dataPointInfo.AdapterId,
                Name = dataPointInfo.Name,
                Type = dataPointInfo.Type,
                Unit = dataPointInfo.Unit,
                Mapping = dataPointInfo.Mapping,
                Source = dataPointInfo.Source
            };

            _uow.DataPointRepository.Update(dataPoint);
            await _uow.CommitAsync();
        }
    }
}
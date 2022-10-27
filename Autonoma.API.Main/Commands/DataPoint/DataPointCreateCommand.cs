using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.Domain.Entities;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.DataPoint
{
    public class DataPointCreateCommand : Command
    {
        public DataPointConfigurationItem DataPoint { get; set; }

        public int CreatedId { get; set; }
    }

    public class DataPointCreateCommandHandler : CommandHandlerAsync<DataPointCreateCommand>
    {
        public DataPointCreateCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(DataPointCreateCommand command)
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

            _uow.DataPointRepository.Create(dataPoint);
            await _uow.CommitAsync();

            command.CreatedId = dataPoint.Id;
        }
    }
}
using Autonoma.Domain;
using Autonoma.Domain.Abstractions;
using Autonoma.Domain.Entities;
using Autonoma.Extensibility.Shared.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Autonoma.Communication.Hosting
{
    /// <summary>
    /// Адаптер, поддерживаемый плагином
    /// </summary>
    internal class PluginAdapter : IDataAdapter
    {
        private readonly AdapterConfiguration _adapterConfiguration;
        private readonly IDataPointService _dataPointService;
        private readonly IAdapterPlugin _targetPlugin;
        private CancellationTokenSource? _cancellationTokenSource;

        public int Id => _adapterConfiguration.Id;

        private int GetInternalDataPointId(InternalDataPoints pointType) => -(Id * 1000) + (int)pointType;

        public WorkState State { get; set; }

        public PluginAdapter(IDataPointService dataPointService, AdapterConfiguration adapterConfiguration, IAdapterPlugin targetPlugin)
        {
            _dataPointService = dataPointService;
            _adapterConfiguration = adapterConfiguration;
            _targetPlugin = targetPlugin;

            // создаем системные точки данных для размещения сведений о состоянии адаптера
            _dataPointService.CreateSystemDataPoint(
                new DataPointConfiguration 
                {
                    AdapterId = Id,
                    Id = GetInternalDataPointId(InternalDataPoints.Health),
                    Type = System.TypeCode.Int32
                }
            );
        }

        public void Dispose()
        {
            _dataPointService.RemoveDataPoint(GetInternalDataPointId(InternalDataPoints.Health));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            await _targetPlugin.StartAsync(_cancellationTokenSource.Token);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => _cancellationTokenSource?.Cancel());
        }
    }
}
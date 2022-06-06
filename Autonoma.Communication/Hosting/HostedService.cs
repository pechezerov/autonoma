using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Autonoma.Communication.Hosting
{
    public abstract class HostedService : IHostedService
    {
        private Task _executingTask = null!;

        private CancellationTokenSource _cts = null!;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _executingTask = ExecuteAsync(_cts.Token);
            return _executingTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask != null)
            {
                _cts.Cancel();
                await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }

}
using Autonoma.Communication.Hosting;
using Autonoma.Core;
using Autonoma.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Autonoma.Communication
{
    public class IdleAdapter : DataAdapterBase<IdleConfiguration>
    {
        public override int AdapterTypeId => Globals.IdleAdapterTypeId;

        public IdleAdapter(IDataPointService dps, AdapterConfiguration config)
                    : base(dps, config)
        {
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class IdleConfiguration
    {
    }
}
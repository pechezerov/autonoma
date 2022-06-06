using PrettyScreen.Configuration;
using PrettyScreen.Core;
using PrettyScreen.Core.Attributes;
using PrettyScreen.Core.Communication;

namespace PrettyScreen.Communication
{
    [AdapterType(AdapterType.Idle)]
    public class IdleAdapter : DataAdapterBase
    {
        public IdleAdapter(IDataPointService dps, AdapterConfiguration config, AdapterType adapterType)
            : base(dps, config, adapterType)
        {

        }

        public override void Start()
        {
            
        }

        public override void Stop()
        {
        }
    }
}
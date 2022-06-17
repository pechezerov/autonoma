using Akka.Event;
using Autonoma.Domain.Entities;
using Autonoma.Model.Akka.Events;

namespace Autonoma.Model.Akka.Actors
{
    public class DataAdapterActor : BaseActor
    {
        public DataAdapterActor(AdapterConfiguration config)
        {
            Config = config;
            Context.System.EventStream.Subscribe<DataPointActorInitializedEvent>(Self);
        }

        public AdapterConfiguration Config { get; }
    }
}

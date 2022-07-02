using Akka.Event;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using Autonoma.Model.Akka.Events;
using Autonoma.Model.Akka.Services;

namespace Autonoma.Model.Akka.Actors
{
    public class DataPointActor : BaseActor
    {
        public DataPointActor(ModelElementConfiguration elementPrototype, AkkaHostService host)
        {
            Host = host;
            DataPointPrototype = elementPrototype;
            Receive<DataValue>(OnUpdate);
            Context.System.EventStream.Subscribe<DataPointActorInitializedEvent>(Self);
        }

        protected override void PreStart()
        {
            Host.ReceiveUpdateCallback(DataPointPrototype.Id, DataValue.NoData);
        }

        public AkkaHostService Host { get; }

        public ModelElementConfiguration DataPointPrototype { get; private set; }

        private void OnUpdate(DataValue value)
        {
            Host.ReceiveUpdateCallback(DataPointPrototype.Id, value);
        }
    }
}

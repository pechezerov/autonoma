using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using Autonoma.Model.Akka.Events;

namespace Autonoma.Model.Akka.Actors
{
    public class DataPointActor : BaseActor
    {
        private DataValue? _lastValue;

        public DataPointActor(DataPointConfiguration dataPointPrototype)
        {
            DataPointPrototype = dataPointPrototype;
            Receive<DataValue>(OnUpdate);
            Context.System.EventStream.Subscribe<DataPointActorInitializedEvent>(Self);
        }

        public DataPointConfiguration DataPointPrototype { get; }

        private void OnUpdate(DataValue value)
        {
            if (value == _lastValue)
                return;

            _lastValue = value;
        }
    }
}

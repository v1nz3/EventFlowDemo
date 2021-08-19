using EventFlow.Aggregates;
using EventFlow.ReadStores;

namespace EventFlowDemo.Example
{
    public class ExampleReadModel : IReadModel, IAmReadModelFor<ExampleAggregate, ExampleId, ExampleEvent>
    {
        public string Id { get; private set; }

        public int MagicNumber { get; private set; }

        public void Apply(
            IReadModelContext context,
            IDomainEvent<ExampleAggregate, ExampleId, ExampleEvent> domainEvent)
        {
            Id = domainEvent.AggregateIdentity.Value;
            MagicNumber = domainEvent.AggregateEvent.MagicNumber;
        }
    }
}

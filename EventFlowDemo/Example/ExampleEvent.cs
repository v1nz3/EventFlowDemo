using EventFlow.Aggregates;

namespace EventFlowDemo.Example
{
    public class ExampleEvent : AggregateEvent<ExampleAggregate, ExampleId>
    {
        public ExampleEvent(int magicNumber)
        {
            MagicNumber = magicNumber;
        }

        public int MagicNumber { get; }
    }
}

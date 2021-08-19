using EventFlow.Core;

namespace EventFlowDemo.Example
{
    public class ExampleId : Identity<ExampleId>
    {
        public ExampleId(string value) : base(value) { }
    }
}

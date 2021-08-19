using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Configuration;
using EventFlow.Extensions;

namespace EventFlowDemo.Example
{
    public class ExampleModule : IModule
    {
        public void Register(IEventFlowOptions eventFlowOptions) =>
            eventFlowOptions.AddDefaults(typeof(ExampleModule).Assembly);
    }
}

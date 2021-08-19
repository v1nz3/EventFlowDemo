using EventFlow;
using EventFlow.Queries;
using EventFlow.ReadStores.InMemory.Queries;
using EventFlowDemo.Example;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;

namespace EventFlowDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamplesController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        

        public ExamplesController(
            ICommandBus commandBus, 
            IQueryProcessor queryProcessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<ActionResult<ExampleReadModel>> GetAll()
        {
            var result =
                await _queryProcessor.ProcessAsync(new InMemoryQuery<ExampleReadModel>(_ => true), CancellationToken.None);

            return Ok(result);
        }

        // GET api/examples/a6e02d4d-871e-4d18-be8a-b647706a2a11
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ExampleReadModel>> GetExample(string id)
        {
            var readModel = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<ExampleReadModel>(id), CancellationToken.None);

            if (readModel == null)
            {
                return NotFound();
            }

            return Ok(readModel);
        }

        // POST api/examples
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Post([FromBody] int value)
        {
            var exampleCommand = new ExampleCommand(ExampleId.New, value);

            var result = await _commandBus.PublishAsync(exampleCommand, CancellationToken.None);
            if (!result.IsSuccess)
            {
                return BadRequest(((FailedExecutionResult)result).Errors);
            }

            return CreatedAtAction(nameof(GetExample), new { id = exampleCommand.AggregateId.Value}, exampleCommand);
        }       
    }
}

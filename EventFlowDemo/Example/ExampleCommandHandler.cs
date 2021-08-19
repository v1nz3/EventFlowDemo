using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using FluentValidation;

namespace EventFlowDemo.Example
{
    public class ExampleCommandHandler : CommandHandler<ExampleAggregate, ExampleId, IExecutionResult, ExampleCommand>
    {
        private readonly IValidator<ExampleCommand> _validator;

        public ExampleCommandHandler(IValidator<ExampleCommand> validator)
        {
            _validator = validator;
        }

        public override Task<IExecutionResult> ExecuteCommandAsync(
            ExampleAggregate aggregate,
            ExampleCommand command,
            CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(command);
            if(!validationResult.IsValid)
            {
                return Task.FromResult(ExecutionResult.Failed(validationResult.Errors.Select(msg => msg.ToString())));
            }
            var executionResult = aggregate.SetMagicNumer(command.MagicNumber);
            return Task.FromResult(executionResult);
        }
    }
}

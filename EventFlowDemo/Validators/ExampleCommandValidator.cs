using EventFlowDemo.Example;
using FluentValidation;

namespace EventFlowDemo.Validators
{
    public class ExampleCommandValidator : AbstractValidator<ExampleCommand>
    {
        public ExampleCommandValidator()
        {
            RuleFor(c => c.MagicNumber).GreaterThan(0);
        }
    }
}

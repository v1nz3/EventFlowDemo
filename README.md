# EventFlowDemo

A working example of how to setup eventflow for a web api application using .net 5.0. I will be making improvements as i learn more about EventFlow.

## Endpoints

[GET] /api/examples - This retrieves all records in the event sotre

[POST] /api/examples - This appends a new example event to the event store

[GET]  /api/examples/example-f134268c-de10-4740-817b-a2627aa3f8b7 - This retrieves a exmaple data record based on the identity id

[POST] /api/datamodels - This rebuilds each data models/projections

[DELETE] /api/datamodels - This deletes all data model records

## Configuration

This basic template uses the `IServiceProvider` and uses the file store for the persisting of the events. The event store is created at the root of the application. The read store is configured to use in-memory so if the application restarts, the read states will be lost.

```csharp
services.AddEventFlow(opt =>
        {
            opt.AddAspNetCore(cfg =>
            {
                cfg.UseDefaults();
            })
            .AddEvents(typeof(ExampleEvent))
            .AddCommands(typeof(ExampleCommand))
            .AddCommandHandlers(typeof(ExampleCommandHandler))
            .UseConsoleLog()
            .UseFilesEventStore(FilesEventStoreConfiguration.Create("./evt-store"))
            .UseInMemoryReadStoreFor<ExampleReadModel>();
        });
```

## Validation

This example uses [FluentValidation](https://docs.fluentvalidation.net/en/latest/installation.html) to validate the input of the `ExampleComandHandler`. Therefore a Validator for the `ExampleCommand` is created and injected into the `ExampleComandHandler`

```csharp
public class ExampleCommandValidator : AbstractValidator<ExampleCommand>
{
    public ExampleCommandValidator()
    {
        RuleFor(c => c.MagicNumber).GreaterThan(0);
    }
}
```

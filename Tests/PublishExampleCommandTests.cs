using EventFlow;
using EventFlow.AspNetCore.Extensions;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.Extensions;
using EventFlow.Queries;
using EventFlowDemo;
using EventFlowDemo.Example;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Tests
{
    public class PublishExampleCommandTests
    {
        private ServiceProvider _serviceProvider;

        [SetUp]
        public async Task Setup()
        {
            var services = new ServiceCollection();
            services.AddValidatorsFromAssembly(typeof(Startup).Assembly);

            services.AddEventFlow(opt =>
            {
                opt.AddAspNetCore(cfg =>
                    {
                        cfg.UseDefaults();
                    })
                    //.AddEvents(typeof(ExampleEvent))
                    //.AddCommands(typeof(ExampleCommand))
                    //.AddCommandHandlers(typeof(ExampleCommandHandler))
                    //.AddDefaults(Assembly.GetExecutingAssembly())
                    .RegisterModule<ExampleModule>()
                    .UseConsoleLog()
                    .UseInMemoryReadStoreFor<ExampleReadModel>();
            });

            _serviceProvider = services.BuildServiceProvider();
            var bootstrapper = _serviceProvider.GetRequiredService<IHostedService>();
            await bootstrapper.StartAsync(CancellationToken.None);
        }

        [TearDown]
        public async Task TearDown()
        {
            var bootstrapper = _serviceProvider.GetRequiredService<IHostedService>();
            await bootstrapper.StopAsync(CancellationToken.None);
            await _serviceProvider.DisposeAsync();
        }

        [Test]
        public async Task Test1()
        {
            var id = ExampleId.New;
            var commandBus = _serviceProvider.GetRequiredService<ICommandBus>();
            var queryProcessor = _serviceProvider.GetRequiredService<IQueryProcessor>(); 
            var exampleCommand = new ExampleCommand(id, 6);

            await commandBus.PublishAsync(exampleCommand, CancellationToken.None);

            var readModel = await queryProcessor.ProcessAsync(new ReadModelByIdQuery<ExampleReadModel>(id), CancellationToken.None);

            readModel.MagicNumber.Should().Be(6);
        }
    }
}
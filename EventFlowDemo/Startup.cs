using System.Reflection;
using EventFlow.AspNetCore.Extensions;
using EventFlow.DependencyInjection.Extensions;
using EventFlow.EventStores.Files;
using EventFlow.Extensions;
using EventFlowDemo.Example;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EventFlowDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventFlowDemo", Version = "v1" });
            });

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddEventFlow(opt =>
            {
                opt.AddAspNetCore(cfg =>
                {
                    cfg.UseDefaults();
                })
                //.AddEvents(typeof(ExampleEvent))
                //.AddCommands(typeof(ExampleCommand))
                //.AddCommandHandlers(typeof(ExampleCommandHandler))
                .AddDefaults(Assembly.GetExecutingAssembly())
                .UseConsoleLog()
                .UseFilesEventStore(FilesEventStoreConfiguration.Create("./evt-store"))
                .UseInMemoryReadStoreFor<ExampleReadModel>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventFlowDemo"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

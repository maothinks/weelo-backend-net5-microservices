using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Weelo.Microservices.Properties.Infrastructure.API.Services;
using GreenPipes;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using System.Linq;
using Weelo.Microservices.Properties.Domain.Interfaces.Repositories;
using Weelo.Microservices.Properties.Domain.DTOS;
using Weelo.Microservices.Properties.Domain;
using System;
using Weelo.Microservices.Properties.Infrastructure.Data.Repositories;

namespace Weelo.Microservices.Properties.Infrastructure.API
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
            // Dependece Injection References
            services.AddScoped<IBaseRepository<ParamsDTO, PaginationMetadataDTO, Property, Guid>, PropertyRepository>();
            services.AddScoped<IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyView, Guid>, PropertyViewsRepository>();
            services.AddScoped<IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyImage, Guid>, PropertyImageRepository>();

            IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfiguration configuration = configBuilder.Build();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<RabbitMQConsumerService>();

                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(configuration.GetSection("RabbitMQ:Host").Value);

                    cfg.ReceiveEndpoint(configuration.GetSection("RabbitMQ:NotifyViewQueue").Value, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(2, 100));
                        e.ConfigureConsumer<RabbitMQConsumerService>(provider);
                    });
                }));
            });

            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Weelo.Microservices.Properties.Infrastructure.API", Version = "v1" });
            });

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weelo.Microservices.Properties.Infrastructure.API v1"));
            }

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = (c) =>
                {
                    var exception = c.Features.Get<IExceptionHandlerFeature>();

                    var statusCode = exception.Error.GetType().Name switch
                    {
                        "ArgumentException" => HttpStatusCode.BadRequest,
                        _ => HttpStatusCode.ServiceUnavailable
                    };

                    c.Response.StatusCode = (int)statusCode;

                    var content = Encoding.UTF8.GetBytes($"ERROR [{ exception.Error.Message }]");
                    c.Response.Body.WriteAsync(content, 0, content.Length);

                    return Task.CompletedTask;
                }
            });

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .WithExposedHeaders("x-pagination")
                .AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseHealthChecks("/health");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

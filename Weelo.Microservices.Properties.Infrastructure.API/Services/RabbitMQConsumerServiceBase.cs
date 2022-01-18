using MassTransit;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Application.Interfaces;
using Weelo.Microservices.Properties.Application.Services;
using Weelo.Microservices.Properties.Domain;
using Weelo.Microservices.Properties.Infrastructure.Data.Contexts;
using Weelo.Microservices.Properties.Infrastructure.Data.Repositories;

namespace Weelo.Microservices.Properties.Infrastructure.API.Services
{
    public class RabbitMQConsumerServiceBase
    {
        /// <summary>
        /// Create Property View Service
        /// </summary>
        /// <returns></returns>
        PropertyViewService CreateService()
        {
            //PropertyContext db = new();
            PropertyViewsRepository repo = new();
            PropertyViewService service = new(repo);
            return service;
        }

        /// <summary>
        /// Consume Message from rabbit MQ and insert into database
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<PropertyView> context)
        {
            var service = CreateService();
            await service.AddAsync(context.Message);
        }
    }
}
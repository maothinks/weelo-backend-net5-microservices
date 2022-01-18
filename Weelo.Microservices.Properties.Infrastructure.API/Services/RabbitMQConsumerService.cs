using MassTransit;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Domain;

namespace Weelo.Microservices.Properties.Infrastructure.API.Services
{
    public class RabbitMQConsumerService : RabbitMQConsumerServiceBase, IConsumer<PropertyView>
    {
    }
}

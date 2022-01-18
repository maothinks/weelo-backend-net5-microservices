using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs.Clients;

namespace Weelo.Microservices.Notifications.SignalrAPI.Services
{
    public class RabbitMQConsumerService : RabbitMQConsumerServiceBase, IConsumer<PropertyView>
    {
        public RabbitMQConsumerService(IHubContext<WeeloHub, IWeeloClient> weeloHub) : base(weeloHub)
        {
        }
    }
}

using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs.Clients;
using Weelo.Microservices.Notifications.SignalrAPI.Models;

namespace Weelo.Microservices.Notifications.SignalrAPI.Services
{
    public class RabbitMQConsumerServiceBase
    {
        private readonly IHubContext<WeeloHub, IWeeloClient> _weeloHub;

        public RabbitMQConsumerServiceBase(IHubContext<WeeloHub, IWeeloClient> weeloHub)
        {
            _weeloHub = weeloHub;
        }

        /// <summary>
        /// Consume Message from rabbit MQ and send to Client using Signalr
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<PropertyView> context)
        {
            WeeloMessage weeloMessage = new();
            weeloMessage.Message = context.Message.PropertyId.ToString();
            weeloMessage.UserId = context.Message.UserId;

            await _weeloHub.Clients.All.ReceiveMessage(weeloMessage);
        }
    }
}
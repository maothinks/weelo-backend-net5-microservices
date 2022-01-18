using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs.Clients;
using Weelo.Microservices.Notifications.SignalrAPI.Models;

namespace Weelo.Microservices.Notifications.SignalrAPI.Hubs
{
    public class WeeloHub : Hub<IWeeloClient>
    {

        public async Task SendMessage(WeeloMessage message)
        {
            await Clients.All.ReceiveMessage(message);
        }
    }
}

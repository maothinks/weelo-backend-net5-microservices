using System.Threading.Tasks;
using Weelo.Microservices.Notifications.SignalrAPI.Models;

namespace Weelo.Microservices.Notifications.SignalrAPI.Hubs.Clients
{
    public interface IWeeloClient
    {
        Task ReceiveMessage(WeeloMessage message);
    }
}

using System;
using System.Collections.Generic;
using Weelo.Microservices.Notifications.API.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Linq;

namespace Weelo.Microservices.Notifications.API.Hubs
{
    public class NotificationsHub : Hub
    {
        private readonly string _botUser;
        private readonly IDictionary<string, UserConnection> _connections;

        public NotificationsHub(IDictionary<string, UserConnection> connections)
        {
            _botUser = "RabbitMQ";
            _connections = connections;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                _connections.Remove(Context.ConnectionId);
                Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has left");
                SendUsersConnected(userConnection.Room);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task JoinRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);

            _connections[Context.ConnectionId] = userConnection;

            await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", _botUser, $"{userConnection.User} has joined {userConnection.Room}");

            await SendUsersConnected(userConnection.Room);
        }

        public async Task SendMessage(string message)
        {

            //await Clients.Group("NotificationsQueue").SendAsync("ReceiveMessage", "99", message);
            await Clients.All.SendAsync("ReceiveMessage", "RabbitMQ", message);

            //if (_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            //{
            //    //await Clients.All.SendAsync("ReceiveMessage", userConnection.User, message);
            //    await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, message);

            //    //await Clients.Group(userConnection.Room).SendAsync("ReceiveMessage", userConnection.User, message);
            //}
        }

        public Task SendUsersConnected(string room)
        {
            var users = _connections.Values
                .Where(c => c.Room == room)
                .Select(c => c.User);

            return Clients.Group(room).SendAsync("UsersInRoom", users);
        }
    }
}

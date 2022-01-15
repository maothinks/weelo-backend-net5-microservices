using Microsoft.Extensions.Caching.Memory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Weelo.Microservices.Notifications.API.Hubs;

namespace Weelo.Microservices.Notifications.API.Services
{
    public class Consumer
    {
        private readonly IMemoryCache _memoryCache;
        private readonly NotificationsHub _hub;
        ConnectionFactory _factory { get; set; }
        IConnection _connection { get; set; }
        IModel _channel { get; set; }

        public Consumer(IMemoryCache memoryCache, NotificationsHub hub)
        {
            _memoryCache = memoryCache;
            _hub = hub;
        }

        public void ReceiveMessageFromQ()
        {
            try
            {
                _factory = new ConnectionFactory()
                {
                    Uri = new Uri("amqp://guest:guest@localhost:5672")
                };

                _connection = _factory.CreateConnection();
                _channel = _connection.CreateModel();

                {
                    _channel.QueueDeclare(queue: "NotificationsQueue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    _channel.BasicQos(prefetchSize: 0, prefetchCount: 3, global: false);

                    var consumer = new EventingBasicConsumer(_channel);
                    consumer.Received += async (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());

                        Dictionary<string, int> messages = null;
                        if (messages == null) messages = new Dictionary<string, int>();

                        //if(messages.Any())
                        //    await _hub.SendMQMessage(messages.OrderBy(m => m.Value).Select(m => m.Key).ToList());
                        await _hub.SendMessage(message);


                        _channel.BasicAck(ea.DeliveryTag, false);
                    };

                    _channel.BasicConsume(queue: "NotificationsQueue",
                                         autoAck: false,
                                         consumer: consumer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} | {ex.StackTrace}");
            }
        }
    }
}

using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weelo.Microservices.Notifications.RabbitMQ_Producer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyViewsController : ControllerBase
    {

        private readonly IBusControl _bus;
        private readonly ILogger<NotifyViewsController> _logger;

        public NotifyViewsController(IBusControl bus, ILogger<NotifyViewsController> logger) {
            _bus = bus;
            _logger = logger;
        }

        /// <summary>
        /// POST api/<NotifyViewsController
        /// </summary>
        /// <param name="propertyViews"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JsonResponse>> Post([FromBody] PropertyView propertyViews)
        {
            try
            {
                // Gets the rabbit details from configuration file
                IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfiguration configuration = configBuilder.Build();
                var rabbitMQDetails = configuration.GetSection("RabbitMQ");

                // PROPERTIES SERVICE
                Uri uri = new Uri(rabbitMQDetails["Host"] + "/" + rabbitMQDetails["NotifyViewQueue"]);

                // Send message to broker
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(propertyViews);

                // SIGNALR SERVICE
                uri = new Uri(rabbitMQDetails["Host"] + "/" + rabbitMQDetails["NotifyViewQueueSignalr"]);

                var endPointSignalr = await _bus.GetSendEndpoint(uri);
                await endPointSignalr.Send(propertyViews);

                return Ok(new JsonResponse() { Success=true, Message = "Success" } );
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

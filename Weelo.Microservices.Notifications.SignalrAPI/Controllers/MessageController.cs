using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs;
using Weelo.Microservices.Notifications.SignalrAPI.Hubs.Clients;
using Weelo.Microservices.Notifications.SignalrAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weelo.Microservices.Notifications.SignalrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<WeeloHub, IWeeloClient> _weeloHub;
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger, IHubContext<WeeloHub, IWeeloClient> weeloHub)
        {
            _weeloHub = weeloHub;
            _logger = logger;
        }

        // POST api/<MessageController>
        [HttpPost]
        public async Task<ActionResult<JsonResponse>> Post([FromBody] string message)
        {
            try
            {
                WeeloMessage weeloMessage = new();
                weeloMessage.Message = message;
                weeloMessage.UserId = 0;

                await _weeloHub.Clients.All.ReceiveMessage(weeloMessage);

                return Ok(new JsonResponse() { Success = true, Message = "Success" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

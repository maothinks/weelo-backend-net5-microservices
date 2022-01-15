using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Weelo.Microservices.Notifications.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weelo.Microservices.Notifications.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        // GET: api/<NotificationsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public ActionResult NotifyView([FromBody] PropertyView view)
        {
            return Ok();
        }
    }
}

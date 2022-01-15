using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Application.Services;
using Weelo.Microservices.Properties.Domain;
using Weelo.Microservices.Properties.Domain.DTOS;
using Weelo.Microservices.Properties.Infrastructure.Data.Contexts;
using Weelo.Microservices.Properties.Infrastructure.Data.Repositories;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weelo.Microservices.Properties.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        PropertyService CreateService()
        {
            PropertyContext db = new ();
            PropertyRepository repo = new(db);
            PropertyService service = new(repo);
            return service;
        }

        // GET: api/<PropertiesController>
        [HttpGet]

        public async Task<ActionResult<List<Property>>> Get([FromQuery] ParamsDTO @params)
        {
            var service = CreateService();

            var paginationMetadata = await service.GetAllMetadataAsync(@params);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(await service.GetAllAsync(@params));
        }

        // GET api/<PropertiesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> Get(Guid id)
        {
            var service = CreateService();
            return Ok(await service.GetByIdAsync(id));
        }

        // POST api/<PropertiesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Property property)
        {
            var service = CreateService();
            await service.AddAsync(property);
            return Ok("Property added successfully!");
        }

        // PUT api/<PropertiesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] Property property)
        {
            var service = CreateService();
            property.PropertyId = id;
            bool result = await service.EditAsync(property);
            return Ok(result? "Property updated successfully!" : "Property was not updated");
        }

        // DELETE api/<PropertiesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var service = CreateService();
            bool result = await service.DeleteAsync(id);
            return Ok(result ? "Property removed successfully!" : "Property was not removed");
        }
    }
}

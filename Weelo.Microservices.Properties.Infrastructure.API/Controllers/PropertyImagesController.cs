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
    public class PropertyImagesController : ControllerBase
    {
        PropertyImageService CreateService()
        {
            PropertyContext db = new();
            PropertyImageRepository repo = new(db);
            PropertyImageService service = new(repo);
            return service;
        }

        PropertyService CreatePropertyService()
        {
            PropertyContext db = new();
            PropertyRepository repo = new(db);
            PropertyService service = new(repo);
            return service;
        }

        // GET: api/<PropertyImagesController>
        [HttpGet]

        public async Task<ActionResult<List<PropertyImage>>> Get([FromQuery] ParamsDTO @params)
        {
            var service = CreateService();

            var paginationMetadata = await service.GetAllMetadataAsync(@params);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(await service.GetAllAsync(@params));
        }

        // GET api/<PropertyImagesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyImage>> Get(Guid id)
        {
            var service = CreateService();
            return Ok(await service.GetByIdAsync(id));
        }

        [HttpGet("{id}")]
        [Route("GetAllByPropertyId/{id}")]
        public async Task<ActionResult<List<PropertyImage>>> GetAllByPropertyId(Guid id)
        {
            var service = CreateService();
            return Ok(await service.GetAllByPropertyIdAsync(id));
        }

        // POST api/<PropertyImagesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PropertyImage propertyImage)
        {
            var service = CreateService();
            var serviceProperty = CreatePropertyService();
            await service.AddAsync(propertyImage);
            
            Property property = await serviceProperty.GetByIdAsync(propertyImage.PropertyId);
            property.CoverPath = propertyImage.FilePath;
            await serviceProperty.EditAsync(property);
            return Ok("Property Image added successfully!");
        }

        // PUT api/<PropertyImagesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] PropertyImage propertyImage)
        {
            var service = CreateService();
            propertyImage.PropertyImageId = id;
            bool result = await service.EditAsync(propertyImage);
            return Ok(result ? "Property Image updated successfully!" : "Property was not updated");
        }

        // DELETE api/<PropertyImagesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var service = CreateService();
            bool result = await service.DeleteAsync(id);
            return Ok(result ? "Property Image removed successfully!" : "Property Image was not removed");
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Weelo.Microservices.Properties.Application.Services;
using Weelo.Microservices.Properties.Domain;
using Weelo.Microservices.Properties.Domain.DTOS;
using Weelo.Microservices.Properties.Domain.Interfaces.Repositories;
using Weelo.Microservices.Properties.Infrastructure.API.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weelo.Microservices.Properties.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly ILogger<PropertiesController> _logger;
        
        // Services will receive IRepos (DI)
        private readonly PropertyService _service;
        private readonly PropertyViewService _serviceViews;

        /// <summary>
        /// Properties Controles
        /// </summary>
        /// <param name="logger"></param>
        public PropertiesController(
            ILogger<PropertiesController> logger, 
            IBaseRepository<ParamsDTO, PaginationMetadataDTO, Property, Guid> propertyRepositoy,
            IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyView, Guid> propertyViewRepositoy)
        {
            _logger = logger;

            // For services (Use Cases Application is not necesary Dependece Injection Only the Repositories
            // the Business Rules will be the same.
            _service = new PropertyService(propertyRepositoy);
            _serviceViews = new PropertyViewService(propertyViewRepositoy);
        }

        /// <summary>
        /// Get properties pagination and filtering based
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        // GET: api/<PropertiesController>
        [HttpGet]

        public async Task<ActionResult<JsonResponse>> Get([FromQuery] ParamsDTO @params)
        {
            try
            {
                // Add pagination to header response
                var paginationMetadata = await _service.GetAllMetadataAsync(@params);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                // get properties
                IList<Property> properties = await _service.GetAllAsync(@params);

                // Add property view to each property
                foreach (Property property in properties)
                {
                    IList<PropertyView> pv = await _serviceViews.GetAllByParentIdAsync(property.PropertyId);
                    property.Views = pv.Count;
                }

                return Ok(new JsonResponse { Success = true, Message = await _service.GetAllAsync(@params) });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get a specific property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<PropertiesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JsonResponse>> Get(Guid id)
        {
            try
            {
                Property property = await _service.GetByIdAsync(id);

                if (property != null) {
                    IList<PropertyView> pv = await _serviceViews.GetAllByParentIdAsync(id);
                    property.Views = pv.Count;
                }

                return Ok(new JsonResponse() { Success = true, Message = property });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Create a new property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        // POST api/<PropertiesController>
        [HttpPost]
        public async Task<ActionResult<JsonResponse>> Post([FromBody] Property property)
        {
            try
            {
                await _service.AddAsync(property);
                return Ok(new JsonResponse { Success = true, Message = "Property added successfully!" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates property
        /// </summary>
        /// <param name="id"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        // PUT api/<PropertiesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<JsonResponse>> Put(Guid id, [FromBody] Property property)
        {
            try
            {
                property.PropertyId = id;
                bool result = await _service.EditAsync(property);

                if (!result)
                {
                    return Ok(new JsonResponse { Success = false, Message = "Property was not updated" });
                }

                return Ok(new JsonResponse { Success = true, Message = "Property updated successfully!" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete a property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<PropertiesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<JsonResponse>> Delete(Guid id)
        {
            try
            {
                bool result = await _service.DeleteAsync(id);

                if (!result)
                {
                    return Ok(new JsonResponse { Success = false, Message = "Property was not removed" });
                }

                return Ok(new JsonResponse { Success = true, Message = "Property removed successfully!" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

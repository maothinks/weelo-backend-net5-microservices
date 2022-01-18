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
using Weelo.Microservices.Properties.Infrastructure.Data.Contexts;
using Weelo.Microservices.Properties.Infrastructure.Data.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weelo.Microservices.Properties.Infrastructure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImagesController : ControllerBase
    {
        private readonly ILogger<PropertyImagesController> _logger;

        // Services will receive IRepos (DI)
        private readonly PropertyImageService _service;
        private readonly PropertyService _serviceProperty;

        /// <summary>
        /// PropertyImages COntrolles
        /// </summary>
        /// <param name="logger"></param>
        public PropertyImagesController(
            ILogger<PropertyImagesController> logger,
            IBaseRepository<ParamsDTO, PaginationMetadataDTO, PropertyImage, Guid> propertyImageRepositoy,
            IBaseRepository<ParamsDTO, PaginationMetadataDTO, Property, Guid> propertyRepositoy)
        {
            _logger = logger;

            // For services (Use Cases Application is not necesary Dependece Injection Only the Repositories
            // the Business Rules will be the same.
            _service = new PropertyImageService(propertyImageRepositoy);
            _serviceProperty = new PropertyService(propertyRepositoy);
        }

        /// <summary>
        /// Get Property Images Pagination and Filtering based
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        // GET: api/<PropertyImagesController>
        [HttpGet]

        public async Task<ActionResult<JsonResponse>> Get([FromQuery] ParamsDTO @params)
        {
            try
            {
                var paginationMetadata = await _service.GetAllMetadataAsync(@params);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                return Ok(new JsonResponse() { Success = true, Message = await _service.GetAllAsync(@params) });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get Image Property By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //GET api/<PropertyImagesController>/5
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<JsonResponse>> GetById(Guid id)
        {
            try
            {
                return Ok(new JsonResponse() { Success = true, Message = await _service.GetByIdAsync(id) });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all images for an specific Property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetAllByPropertyId/{id}")]

        public async Task<ActionResult<JsonResponse>> GetAllByPropertyId(Guid id)
        {
            try
            {
                return Ok(new JsonResponse() { Success = true, Message = await _service.GetAllByParentIdAsync(id) });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates Property Image
        /// </summary>
        /// <param name="propertyImage"></param>
        /// <returns></returns>
        // POST api/<PropertyImagesController>
        [HttpPost]
        public async Task<ActionResult<JsonResponse>> Post([FromBody] PropertyImage propertyImage)
        {
            try
            {
                await _service.AddAsync(propertyImage);

                // Creates the image and update the cover image for the parent property
                Property property = await _serviceProperty.GetByIdAsync(propertyImage.PropertyId);
                property.CoverPath = propertyImage.FilePath;
                await _serviceProperty.EditAsync(property);
                return Ok(new JsonResponse() { Success = true, Message = "Property Image added successfully!" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates an image property
        /// </summary>
        /// <param name="id"></param>
        /// <param name="propertyImage"></param>
        /// <returns></returns>
        // PUT api/<PropertyImagesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<JsonResponse>> Put(Guid id, [FromBody] PropertyImage propertyImage)
        {
            try
            {
                propertyImage.PropertyImageId = id;
                bool result = await _service.EditAsync(propertyImage);

                if (!result)
                {
                    return Ok(new JsonResponse { Success = false, Message = "Property Image was not updated" });
                }

                return Ok(new JsonResponse { Success = true, Message = "Property Image updated successfully!" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delerte image Property
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<PropertyImagesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<JsonResponse>> Delete(Guid id)
        {
            try
            {
                bool result = await _service.DeleteAsync(id);

                if (!result)
                {
                    return Ok(new JsonResponse { Success = false, Message = "Property Image was not removed" });
                }

                return Ok(new JsonResponse { Success = true, Message = "Property Image removed successfully!" });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}

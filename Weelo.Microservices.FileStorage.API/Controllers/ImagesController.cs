
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Weelo.Microservices.FileStorage.API.DTOS;
using Weelo.Microservices.FileStorage.API.Services;

namespace Weelo.Microservices.FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly IFirebaseManagerServiceBase _fireBaseManager;

        /// <summary>
        /// Image COntroller Constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        public ImagesController(ILogger<ImagesController> logger, IFirebaseManagerServiceBase fireBaseManager) { 
            _logger = logger;
            _fireBaseManager = fireBaseManager;
        }

        /// <summary>
        /// Upload Image get an object with a base64 image
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Url image upload to Firebase Storage</returns>
        [HttpPost]
        [Route("UploadImage")]
        public async Task<ActionResult<JsonResponse>> UploadIamge([FromBody] ImageDTO model)
        {
            try
            {
                string imageFromFirebase = await UploadImage(model);
                return Ok(new JsonResponse { Success = true, Message = imageFromFirebase });
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Upload Image Private fnction to call the firebase manager service
        /// </summary>
        /// <param name="model"></param>
        /// <returns>URL image uploaded</returns>
        private async Task<string> UploadImage(ImageDTO model)
        {
            var imageFromBase64ToStream = _fireBaseManager.ConvertBase64ToStream(model.Image);
            var imageStream = imageFromBase64ToStream.ReadAsStream();

            string imageFromFirebase = await _fireBaseManager.UploadImage(imageStream, model);
            return imageFromFirebase;
        }
    }
}

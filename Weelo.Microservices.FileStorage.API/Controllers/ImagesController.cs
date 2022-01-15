
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weelo.Microservices.FileStorage.API.DTOS;
using Weelo.Microservices.FileStorage.API.Services;

namespace Weelo.Microservices.FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadIamge([FromBody] ImageDTO model)
        {
            string imageFromFirebase = await UploadImage(model);
            return Ok(imageFromFirebase);
        }

        private static async Task<string> UploadImage(ImageDTO model)
        {
            var imageFromBase64ToStream = FirebaseManagerService.ConvertBase64ToStream(model.Image);
            var imageStream = imageFromBase64ToStream.ReadAsStream();

            string imageFromFirebase = await FirebaseManagerService.UploadImage(imageStream, model);
            return imageFromFirebase;
        }
    }
}

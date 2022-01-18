using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Weelo.Microservices.FileStorage.API.DTOS;

namespace Weelo.Microservices.FileStorage.API.Services
{
    public interface IFirebaseManagerServiceBase
    {
        public StreamContent ConvertBase64ToStream(string imageFromRequest);
        public Task<string> UploadImage(Stream stream, ImageDTO imageDTO);
    }
}
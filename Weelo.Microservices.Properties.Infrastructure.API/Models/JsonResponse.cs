namespace Weelo.Microservices.Properties.Infrastructure.API.Models
{
    /// <summary>
    /// Class used to response data in the rest services
    /// </summary>
    public class JsonResponse
    {
        public bool Success { get; set; }
        public object Message { get; set; }
    }
}

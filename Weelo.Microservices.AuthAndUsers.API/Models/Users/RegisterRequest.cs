using System.ComponentModel.DataAnnotations;

namespace Weelo.Microservices.AuthAndUsers.API.Models.Users
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public System.DateTime BirthDate { get; set; }
        public string Address { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
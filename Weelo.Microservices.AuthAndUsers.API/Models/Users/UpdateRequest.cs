namespace Weelo.Microservices.AuthAndUsers.API.Models.Users
{
    public class UpdateRequest
    {
        public string FirstName { get; set; }
        public string PhotoPath { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Address { get; set; }

        public string Role { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
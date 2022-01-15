namespace Weelo.Microservices.AuthAndUsers.API.Models.Users
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string PhotoPath { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool IsOwner { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
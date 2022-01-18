using System;
using System.Text.Json.Serialization;

namespace Weelo.Microservices.AuthAndUsers.API.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Username { get; set; }
        public string PhotoPath { get; set; }
        public string Address { get; set; }
        public bool IsOwner { get; set; }

        public string Role { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
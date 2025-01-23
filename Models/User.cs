using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }
        
        public string? Address { get; set; }


        public string? PostalCode { get; set; }


        public bool ActiveMember { get; set; } = true;

        public DateOnly DateOfBirth { get; set; }

        public DateOnly RegistrationDate { get; set; }

        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }

    }
}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ERole
    {
        Admin,
        Member,
        Staff
    }

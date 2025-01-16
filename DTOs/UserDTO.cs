using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApplication1.Validators;

namespace WebApplication1.Models
{
    public class UserDTOIn
    {

        [Required, StringLength(50), NoSpecialChars]
        public string? FirstName { get; set; }

        [Required, StringLength(50), NoSpecialChars]
        public string? LastName { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        [EnumDataType(typeof(ERole))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ERole Role { get; set; }

        public string? PostalCode { get; set; }

        [Required]
        public bool ActiveMember { get; set; } = true;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public DateOnly RegistrationDate { get; set; }

    }


    public class UserDTOOut
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public ERole Role { get; set; }
        public string? Country { get; set; }

        public UserDTOOut(
            string id, 
            string? firstName, 
            string? lastName, 
            string? email,
            ERole role,
            string? country
        )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Role = role;
            Country = country;
        }
    }


}

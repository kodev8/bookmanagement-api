using System.ComponentModel.DataAnnotations;
using WebApplication1.Validators;

namespace WebApplication1.Models
{
    public class UserDTOIn
    {

        [Required, StringLength(50), NoSpecialChars]
        public string? FirstName { get; set; }

        [Required, StringLength(50), NoSpecialChars]
        public string? LastName { get; set; }

        [Required, StringLength(50), NoSpecialChars(AllowableChars= "_")]
        public string? UserName { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? Country { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, Password]
        public string? Password { get; set; }

        [Required]
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
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? Country { get; set; }

        public UserDTOOut(
            string id, 
            string? firstName, 
            string? lastName, 
            string? userName,
            string? email,
            string? role,
            string? country
        )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            Role = role;
            Country = country;
        }
    }

        public record LoginDTO(string Email, string Password);
        public record LoginResponseDTO(string Token, string Email, string Role);
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Validators;

namespace WebApplication1.Models
{
    public class User : IdentityUser
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
        public ERole Role { get; set; }
        	

        public string? PostalCode { get; set; }

        [Required]
        public bool IsAdmin { get; set; } = false;

        [Required]
        public bool ActiveMember { get; set; } = false;

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public DateOnly RegistrationDate { get; set; }

        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }

    }

    public enum ERole
    {
        Admin,
        Member,
        Staff
    }
}

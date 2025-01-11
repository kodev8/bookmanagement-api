using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Validators;

namespace WebApplication1.Models
{
    public class UserDTOIn
    {

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }


        public ERole Role { get; set; }


        public string? PostalCode { get; set; }

        public bool IsAdmin { get; set; } = false;

        public bool ActiveMember { get; set; } = false;

        public DateOnly DateOfBirth { get; set; }

        public DateOnly RegistrationDate { get; set; }

    }


    public class UserDTOOut
    {
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }

        public UserDTOOut(string id, string? firstName, string? lastName, string? city, string? country, string? address)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Country = country;
            Address = address;
        }
    }


}

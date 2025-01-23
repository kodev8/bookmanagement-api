using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AuthorDTOIn
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [StringLength(500)]
        public string? Biography { get; set; }
    }

    public class AuthorDTOOut
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string? Biography { get; set; }

        public AuthorDTOOut(int id, string name, string biography)
        {
            Id = id;
            Name = name;
            Biography = biography;
        }
    }

    public class BookAuthorDTOIn
    {
        public int Id { get; set; }
    }
}

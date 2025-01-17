using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class BookDTOIn
    {
        [Required, StringLength(50)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required, Validators.NoSpecialChars]
        public string? Author { get; set; }

        [Required]
        public Genre[]? Genres { get; set; }

        [Required]
        public DateOnly PublicationDate { get; set; }

        [Required, Range(1, 1000)]

        public int NumberOfPages { get; set; }
    }

    public class BookDTOOut
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public Genre[]? Genres { get; set; }
        public DateOnly PublicationDate { get; set; }
        public int NumberOfPages { get; set; }

        public BookDTOOut(int id, string? title, string? description, string? author, Genre[]? genres, DateOnly publicationDate, int numberOfPages)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
            Genres = genres;
            PublicationDate = publicationDate;
            NumberOfPages = numberOfPages;
        }
    }
} 
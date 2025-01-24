using System.ComponentModel.DataAnnotations;
namespace WebApplication1.Models
{
    public class BookDTOIn
    {
        [Required, StringLength(50, MinimumLength = 1)]
        public string? Title { get; set; }

        [Required, StringLength(13)]
        public string? Isbn { get; set; }

        [StringLength(maximumLength: 500)]
        public string? Description { get; set; }

        [Required]
        public string? Language { get; set; }

        [Required]
        public string? Publisher { get; set; }

        // [Required, NoSpecialChars(AllowableChars = "'.")]
        // authors must be added before adding a book, to allow adding their biography
        [Required]
        public int[]? Authors { get; set; }

        // [Required]
        // string of genre ids
        public EGenre[]? Genres { get; set; }

        [Required]
        public DateOnly PublicationDate { get; set; }

        [Required, Range(1, 10000)]
        public int TotalPages { get; set; }

        [Required, Range(1, 10000)]
        public int AvailableCopies { get; set; }

        [Required]
        public DateTime AddedDate { get; set; }
    }

    public class BookDTOOut
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        // string of author names
        public string[]? Authors { get; set; }
        // string of genre names
        public string[]? Genres { get; set; }
        public DateOnly PublicationDate { get; set; }
        public int TotalPages { get; set; }

        public BookDTOOut(
            int id,
            string? title,
            string? description,
            string[]? authors,
            string[]? genres,
            DateOnly publicationDate, int totalPages)
        {
            Id = id;
            Title = title;
            Description = description;
            Authors = authors;
            Genres = genres;
            PublicationDate = publicationDate;
            TotalPages = totalPages;
        }
    }
}
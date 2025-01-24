using System.ComponentModel.DataAnnotations;
using WebApplication1.Validators;
namespace WebApplication1.Models
{
    public class BookLoanDTOIn
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public string? UserId { get; set; }

        public string? Notes { get; set; }

    }


    public class BookLoanDTOOut
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string? UserId { get; set; } 
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public EStatus Status { get; set; }
        public string? Notes { get; set; }

        public BookLoanDTOOut(int id, int bookId, string userId, DateTime dueDate, DateTime? returnDate, EStatus status, string? notes)
        {
            Id = id;
            BookId = bookId;
            UserId = userId;
            DueDate = dueDate;
            ReturnDate = returnDate;
            Status = status;
            Notes = notes;
        }
    }



        
}
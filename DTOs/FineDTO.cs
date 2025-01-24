using System.ComponentModel.DataAnnotations;
using WebApplication1.Validators;
using WebApplication1.Services;
namespace WebApplication1.Models
{
    public class FineDTOIn
    {
        [Required]
        public int BookLoanId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public decimal Amount { get; set; }

        public DateTime? PaidDate { get; set; }

        public string? Reason { get; set; }

    }

    public class FineDTOInPartial
    {
        public DateTime PaidDate { get; set; }
        public EFineStatus Status { get; set; }
        public string? UserId { get; set; }
    }

    public class FineDTOOut
    {
        public int Id { get; set; }
        public int BookLoanId { get; set; }
        public string? UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public EFineStatus Status { get; set; }
        public string? Reason { get; set; }

        public FineDTOOut(int id, int bookLoanId, string? userId, decimal amount, DateTime issuedDate, DateTime? paidDate, EFineStatus status, string? reason)
        {
            Id = id;
            BookLoanId = bookLoanId;
            UserId = userId;
            Amount = amount;
            IssuedDate = issuedDate;
            PaidDate = paidDate;
            Status = status;
            Reason = reason;
        }
    }



        
}
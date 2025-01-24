
using WebApplication1.Services;

namespace WebApplication1.Models;

public partial class Fine
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int BookLoanId { get; set; }

    public decimal Amount { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime IssuedDate { get; set; } = DateTime.Now;

    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);
    public DateTime? PaidDate { get; set; }

    public string Status { get; set; } = EFineStatus.Pending.ToString();

    public virtual BookLoan BookLoan { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication1.Models;

public partial class BookLoan
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public string? UserId { get; set; }

    // the date the book was borrowed
    public DateTime BorrowDate { get; set; }

    // the date the book is due to be returned
    public DateTime DueDate { get; set; }

    // the date the book was returned
    public DateTime? ReturnDate { get; set; }

    public string Status { get; set; } = EStatus.Active.ToString();

    public string? Notes { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    
}

[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EStatus { Active, Returned, Overdue }

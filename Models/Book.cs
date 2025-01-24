using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace WebApplication1.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly PublicationDate { get; set; }

    public string Publisher { get; set; } = null!;

    public int AvailableCopies { get; set; }

    public DateTime AddedDate { get; set; }

    public DateTime? LastUpdated { get; set; }

    public string Language { get; set; } = null!;

    public int TotalPages { get; set; }

    public bool IsRemoved { get; set; } = false;

    public virtual ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

}



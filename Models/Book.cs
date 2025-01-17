namespace WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Validators;

public class Book
{
    [Required]
    public int Id { get; set; }

    [Required, StringLength(50)]
    public string? FullTitle { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required, NoSpecialChars]
    public string? Author { get; set; }

    [Required]
    public Genre[]? Genres { get; set; }

    [Required]
    public DateOnly PublicationDate { get; set; }

    [Required, Range(1, 1000)]
    public int NumberOfPages { get; set; }


    public override string ToString()
    {
        string Base =$"Id: {Id}\nTitle: {FullTitle}\nDescription: {Description}\nAuthor: {Author}\nPubDate: {PublicationDate}\n";

        if (Genres!=null)
        {
            Base += $"Genres: { BuildGenres()}\n";
        }

        return Base;
    }

    private string BuildGenres()
    {
        if (Genres != null && Genres.Length > 0)
        {
           return string.Join(", ", Genres);
        }
        else
        {
            return "No Genres Yet!";
        }
    }
}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Genre
    {
        Fiction,
        NonFiction,
        Fantasy,
        SciFi,
        Mystery,
        Romance,
        Horror,
        Thriller,
        Comedy,
        Drama,
        Action,
        Adventure,
        Biography,
        Autobiography,
        History,
        Science,
        Math,
        Philosophy,
        Religion,
        SelfHelp,
        Health,
        Fitness,
        Cooking,
        Travel,
        Guide,
        Children,
        YoungAdult,
        Adult,
        Other
             
    }

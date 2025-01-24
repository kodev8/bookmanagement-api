using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebApplication1.Models;

// no service for genres as they are hardcoded in the db
public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    
}

 [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EGenre
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

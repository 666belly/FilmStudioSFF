using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public interface ICreateFilm
    {
        string Title { get; set; }
        string Description { get; set; }
        int AvailableCopies { get; set; }
        string Genre { get; set; }
        string Director { get; set; }
        int Year { get; set; }
        bool IsAvailable { get; set; }
    }
}
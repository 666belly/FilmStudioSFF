using System.Collections.Generic;
using FilmStudioSFF.Models;

public interface ICreateFilm
{
    string Title { get; set; }
    string Description { get; set; }
    int AvailableCopies { get; set; }
    int FilmId { get; set;}
    List<FilmCopy> FilmCopies { get; set; }
    string Genre { get; set; }
    string Director { get; set; }
    int Year { get; set; }
    bool IsAvailable { get; set; }
}
using System.Collections.Generic;
using FilmStudioSFF.Models;

public interface IFilm
{
    int FilmId { get; set; }
    string Title { get; set; }
    string Genre { get; set; }
    string Director { get; set; }
    int Year { get; set; }
    string Description { get; set; }
}
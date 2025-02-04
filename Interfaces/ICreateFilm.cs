using System.Collections.Generic;
using FilmStudioSFF.Models;

public interface IFilm
{
    int FilmId { get; set; }
    string Title { get; set; }
    string Description { get; set; }
    int AvailableCopies { get; set; }
    List<FilmCopy> FilmCopies { get; set; }
}
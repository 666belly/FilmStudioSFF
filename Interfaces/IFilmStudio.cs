using System.Collections.Generic;
using FilmStudioSFF.Models;

public interface IFilmStudio
{
    int FilmStudioId { get; set; }
    string Name { get; set; }   
    string City { get; set; }   
    public string Email { get; set; }
    List<FilmCopy> RentedFilmCopies { get; set; }
}
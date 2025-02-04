using System.Collections.Generic;
using FilmStudioSFF.Models;

public interface IFilmStudio
{
    int FilmStudioId { get; set; }
    string Name { get; set; }   
    string City { get; set; }   
    public string Username { get; set; }
    List<FilmCopy> RentedFilms { get; set; }
}
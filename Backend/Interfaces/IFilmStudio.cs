using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public interface IFilmStudio
    {
        int FilmStudioId { get; set; }
        string Name { get; set; }   
        string City { get; set; }   
        string Role { get; set; } 
        string Password { get; set; }
        string Username { get; set; }
        List<FilmCopy> FilmCopies { get; set; } 
    }
}
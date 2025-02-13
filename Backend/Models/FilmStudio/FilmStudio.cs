using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
public class FilmStudio : IFilmStudio
{
    public int FilmStudioId { get; set; }
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Role { get; set; }
    public required string Email { get; set; }
    public required string City { get; set; }
    public required string Password { get; set; }

    // Navigation property for RentedFilms
    public List<FilmCopy> FilmCopies { get; set; } = new List<FilmCopy>();
    public List<FilmCopy> RentedFilms { get; set; } = new List<FilmCopy>(); 
}


}
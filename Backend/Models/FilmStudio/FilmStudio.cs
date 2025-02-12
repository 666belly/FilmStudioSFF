using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
public class FilmStudio : IRegisterFilmStudio
{
    public int FilmStudioId { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; } 
    public string Role { get; set; } = "filmstudio"; 
    public List<FilmCopy> RentedFilms { get; set; } = new List<FilmCopy>();        
    public required string Email { get; set; }
}


}
using FilmStudioSFF.Models;

public class FilmStudioDTO : IFilmStudio
{
    public int FilmStudioId { get; set; }
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Role { get; set; }
    public required string Email { get; set; }

    public string? City { get; set; } 
    public List<FilmCopy>? RentedFilms { get; set; } 


}

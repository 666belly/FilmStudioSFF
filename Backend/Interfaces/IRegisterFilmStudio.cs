using System.Collections.Generic;
using FilmStudioSFF.Models;

public interface IRegisterFilmStudio
{
    string Name { get; set; }
    string Email { get; set; }
    string Username { get; set; }
    string Role { get; set; }
}
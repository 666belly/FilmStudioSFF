using System.Collections.Generic;
using FilmStudioSFF.Models;
using FilmStudioSFF.Interfaces;

namespace FilmStudioSFF.Models
{
    public class FilmStudioLogin
    {
    public required string Username { get; set; }
    public required string Password { get; set; }
    }
}
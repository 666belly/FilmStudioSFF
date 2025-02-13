using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public class FilmStudioRegisterDTO : IRegisterFilmStudio
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}
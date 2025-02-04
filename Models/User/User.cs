using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; } //admin or filmstudio
    }
}
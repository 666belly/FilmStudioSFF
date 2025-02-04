using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } //admin or filmstudio
    }
}
using System.Collections.Generic;
using FilmStudioSFF.Models;
using FilmStudioSFF.Interfaces;

namespace FilmStudioSFF.Models
{
    public class UserRegister : IUserRegister
    {

        public int UserId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}
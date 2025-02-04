using system.Collections.Generic;
using FilmStudioSFF.Models;

namespace FilmStudioSFF.Interfaces
{
    public interface IUser
    {
        int UserId { get; set; }
        string Username { get; set; }
        string Role { get; set; }
        string Token { get; set; }
    }
}
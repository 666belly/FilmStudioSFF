using FilmStudioSFF.Models;
using FilmStudioSFF.Services;
using System.Collections.Generic;   

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
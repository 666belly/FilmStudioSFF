using System.Collections.Generic;
using FilmStudioSFF.Models;

namespace FilmStudioSFF.Interfaces
{
    public interface IUserRegister
    {
        string Username { get; set; }
        string Password { get; set; }
        bool IsAdmin { get; set; }
    }
}
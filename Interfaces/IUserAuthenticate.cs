using System.Collections.Generic;
using System.Threading.Tasks;
using FilmStudioSFF.Models; 

namespace FilmStudioSFF.Interfaces
{
    public interface IUserAuthenticate
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}
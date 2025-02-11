using FilmStudioSFF.Models;
using FilmStudioSFF.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FilmStudioSFF.Services
{
    public class FilmStudioService
    {
        private readonly FilmStudioDbContext _context;

        public FilmStudioService(FilmStudioDbContext context)
        {
            _context = context;
        }

        // Registrera en ny filmstudio
        public FilmStudio? RegisterFilmStudio(IRegisterFilmStudio registerFilmStudio)
        {
            if (_context.FilmStudios.Any(fs => fs.Name == registerFilmStudio.Name))
            {
                throw new InvalidOperationException("En filmstudio med samma namn finns redan.");
            }

            var newStudio = new FilmStudio
            {
                Name = registerFilmStudio.Name,
                City = registerFilmStudio.City,
                Username = registerFilmStudio.Username,
                Email = registerFilmStudio.Email,
                Password = registerFilmStudio.Password,
                Role = registerFilmStudio.Role,
                RentedFilms = new List<FilmCopy>()
            };

            newStudio.Password = BCrypt.Net.BCrypt.HashPassword(newStudio.Password);

            _context.FilmStudios.Add(newStudio);
            _context.SaveChanges();
            return newStudio;
        }

        // Logga in en filmstudio   
        public FilmStudio? FilmStudioLogin(string username, string password)
        {
            var studio = _context.FilmStudios.FirstOrDefault(s => s.Username == username);
            if (studio != null && BCrypt.Net.BCrypt.Verify(password, studio.Password))
            {
                return studio;
            }
            return null;
        }

        // Hämta en specifik filmstudio baserat på ID
        public FilmStudio? GetFilmStudioById(int id)
        {
            var studio = _context.FilmStudios
                .Include(fs => fs.RentedFilms) 
                .FirstOrDefault(fs => fs.FilmStudioId == id);
            return studio;
        }

        // // Hämta alla filmstudios
        public List<FilmStudioDTO> GetAllFilmStudios(string? userRole, bool includeFullDetails)
        {
            var studios = _context.FilmStudios
                .Include(studio => studio.RentedFilms)  // Eager load related data
                .ToList();

            if (includeFullDetails && userRole == "admin")
            {
                // Return full details for admin including City and RentedFilms
                return studios.Select(studio => new FilmStudioDTO
                {
                    FilmStudioId = studio.FilmStudioId,
                    Name = studio.Name,
                    Username = studio.Username,
                    Role = studio.Role,
                    Email = studio.Email,
                    City = studio.City,              
                    RentedFilms = studio.RentedFilms  
                }).ToList();  // List<IFilmStudio>
            }
            else
            {
                // For non-admin, return a reduced version without City and RentedFilms
                return studios.Select(studio => new FilmStudioDTO
                {
                    FilmStudioId = studio.FilmStudioId,
                    Name = studio.Name,
                    Username = studio.Username,
                    Role = studio.Role,
                    Email = studio.Email
                    // City and RentedFilms are excluded for non-admin users
                }).ToList();  // List<IFilmStudio>
            }
        }
        // Hämta alla hyrda filmkopior för en specifik filmstudio
        public List<FilmCopy> GetRentedFilms(int id)
        {
            var studio = _context.FilmStudios
                .Include(fs => fs.RentedFilms)
                .FirstOrDefault(fs => fs.FilmStudioId == id);
            return studio?.RentedFilms ?? new List<FilmCopy>();
        }

        // Hyr ut en filmkopia till en filmstudio
        public bool RentFilmToStudio(int studioId, FilmCopy filmCopy)
        {
            var studio = _context.FilmStudios.FirstOrDefault(fs => fs.FilmStudioId == studioId);
            if (studio == null) 
            {
                Console.WriteLine($"Film studio with ID {studioId} not found.");
                return false;
            }

            studio.RentedFilms.Add(filmCopy);
            return true;
        }


        public bool ReturnRequest(int studioId, int filmCopyId)
        {
            var studio = _context.FilmStudios
                .Include(fs => fs.RentedFilms)
                .FirstOrDefault(fs => fs.FilmStudioId == studioId);

            if (studio == null)
            {
                Console.WriteLine($"Filmstudio med ID {studioId} hittades inte.");
                return false;
            }

            var filmCopy = studio.RentedFilms.FirstOrDefault(fc => fc.FilmCopyId == filmCopyId);
            if (filmCopy == null)
            {
                Console.WriteLine($"Film med ID {filmCopyId} hittades inte i hyrda filmer.");
                return false;
            }

            studio.RentedFilms.Remove(filmCopy);
            _context.SaveChanges();
            return true;
        }

        internal object GetAllFilmCopies()
        {
            throw new NotImplementedException();
        }

        // private int? GetAuthenticatedStudioId(ClaimsPrincipal user)
        // {
        //     var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        //     if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int studioId))
        //     {
        //         return studioId;
        //     }
        //     return null;
        // }


    }
}

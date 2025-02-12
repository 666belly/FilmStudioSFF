using FilmStudioSFF.Models;
using FilmStudioSFF.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;

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
        public FilmStudioDTO? GetFilmStudioById(int id, string? userRole, bool includeFullDetails)
        {
            var studio = _context.FilmStudios
                .Include(studio => studio.RentedFilms)  // Eager load related data
                .FirstOrDefault(studio => studio.FilmStudioId == id);
        
            if (studio == null)
            {
                return null;
            }
        
            if (includeFullDetails && userRole == "admin")
            {
                // Return full details for admin including City and RentedFilms
                return new FilmStudioDTO
                {
                    FilmStudioId = studio.FilmStudioId,
                    Name = studio.Name,
                    Username = studio.Username,
                    Role = studio.Role,
                    Email = studio.Email,
                    City = studio.City,              
                    RentedFilms = studio.RentedFilms  
                };
            }
            else
            {
                // For non-admin, return a reduced version without City and RentedFilms
                return new FilmStudioDTO
                {
                    FilmStudioId = studio.FilmStudioId,
                    Name = studio.Name,
                    Username = studio.Username,
                    Role = studio.Role,
                    Email = studio.Email
                    // City and RentedFilms are excluded for non-admin users
                };
            }
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
            // Hitta filmstudion
            var studio = _context.FilmStudios.Include(fs => fs.RentedFilms).FirstOrDefault(fs => fs.FilmStudioId == studioId);
            if (studio == null)
            {
                return false;
            }

            // Kontrollera om filmkopian finns och inte är redan uthyrd
            var filmCopyToRent = _context.FilmCopies.FirstOrDefault(fc => fc.FilmCopyId == filmCopy.FilmCopyId && !fc.IsRented);
            if (filmCopyToRent == null)
            {
                return false; // Filmkopian är redan uthyrd eller finns inte
            }

            // Lägg till filmkopian till filmstudions hyrda filmer och uppdatera filmkopian
            studio.RentedFilms.Add(filmCopyToRent);
            filmCopyToRent.IsRented = true;  

            _context.SaveChanges();  
            return true;
        }
        public List<FilmCopy> GetRentalsForStudio(int studioId)
        {
            var filmCopyIds = _context.Rentals
                                    .Where(r => r.StudioId == studioId)
                                    .Select(r => r.FilmCopyId) 
                                    .ToList();

            if (!filmCopyIds.Any())
            {
                Console.WriteLine($"Inga uthyrningar hittades för studioId {studioId}");
            }

            return _context.FilmCopies
                        .Where(fc => filmCopyIds.Contains(fc.FilmCopyId))
                        .ToList(); 
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

    }
}

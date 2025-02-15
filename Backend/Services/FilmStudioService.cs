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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationService _authenticationService;

        private readonly ILogger<FilmStudioService> _logger;



        public FilmStudioService(FilmStudioDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<FilmStudioService> logger, AuthenticationService authenticationService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _authenticationService = authenticationService;

        }

        //Rent film to studio, save to list// FilmStudioService.cs

        public bool RentFilmToStudio(int filmId, int studioId)
        {
            Console.WriteLine($"RentFilmToStudio called with filmId: {filmId}, studioId: {studioId}");

            // Check if the user is authenticated
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null)
            {
                Console.WriteLine("Unauthorized access: No user found.");
                return false;
            }

            // Check user ID and role
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = user.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;

            Console.WriteLine($"User ID: {userIdClaim}, Role: {userRole}");

            // Check if the user has the correct role
            if (userRole == null || !userRole.Equals("filmstudio", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Unauthorized access: User is not a FilmStudio.");
                return false;
            }

            // Check if the user is trying to rent for their own studio
            if (int.Parse(userIdClaim) != studioId)
            {
                Console.WriteLine($"Unauthorized: User {userIdClaim} tried to rent for studio {studioId}");
                return false;
            }

            // Get the film and check if it exists
            var film = _context.Films
                .Include(f => f.FilmCopies)
                .FirstOrDefault(f => f.FilmId == filmId);

            if (film == null)
            {
                Console.WriteLine($"Film with ID {filmId} not found.");
                return false;
            }

            Console.WriteLine($"Film found: {film.Title}");

            // Check if the film is available
            if (!film.IsAvailable)
            {
                Console.WriteLine("Film is not available for rental.");
                return false;
            }

            // Get the studio and check if it exists
            var studio = _context.FilmStudios
                .Include(s => s.RentedFilms)
                .FirstOrDefault(s => s.FilmStudioId == studioId);

            if (studio == null)
            {
                Console.WriteLine($"Studio with ID {studioId} not found.");
                return false;
            }

            // Check if the studio has already rented the film
            var rentedBefore = studio.RentedFilms.Any(fc => fc.FilmId == filmId);
            Console.WriteLine($"Studio has already rented this film: {rentedBefore}");

            if (rentedBefore)
            {
                return false; // If the film is already rented by the studio, abort the process
            }

            // Get all copies of the film
            var filmCopies = film.FilmCopies.Where(fc => fc.FilmId == filmId).ToList();
            Console.WriteLine($"Found {filmCopies.Count} copies of the film.");

            // Check if any of the copies are available (IsRented == false)
            var availableCopy = filmCopies.FirstOrDefault(fc => fc.IsRented == false);
            if (availableCopy == null)
            {
                Console.WriteLine("No available copies for rental.");
                return false; // No available copy to rent
            }

            Console.WriteLine($"Renting film copy ID {availableCopy.FilmCopyId} to studio {studioId}");

            // Mark the copy as rented
            availableCopy.IsRented = true;

            // Add the rental to the database
            _context.Rentals.Add(new Rental { FilmCopyId = availableCopy.FilmCopyId, StudioId = studioId });
            _context.SaveChanges();

            Console.WriteLine("Film rented successfully.");
            return true;
        }


        // Register new filmstuido
        public IRegisterFilmStudio RegisterFilmStudio(FilmStudio filmStudio)
        {
            // Hash the password before saving
            filmStudio.Password = BCrypt.Net.BCrypt.HashPassword(filmStudio.Password);

            _context.FilmStudios.Add(filmStudio);
            _context.SaveChanges();

            var filmStudioRegisterDTO = new FilmStudioRegisterDTO
            {
                Username = filmStudio.Username,
                Name = filmStudio.Name,
                Email = filmStudio.Email,
                Role = filmStudio.Role
            };

            return filmStudioRegisterDTO;
        }

        public FilmStudioLoginResponse? FilmStudioLogin(string username, string password)
        {
            var studio = _context.FilmStudios.FirstOrDefault(s => s.Username == username);
            if (studio != null && BCrypt.Net.BCrypt.Verify(password, studio.Password))
            {
                var token = _authenticationService.GenerateJwtToken(studio.Username, studio.Role, studio.FilmStudioId);
                return new FilmStudioLoginResponse
                {
                    FilmStudioId = studio.FilmStudioId,
                    Username = studio.Username,
                    Name = studio.Name,
                    City = studio.City,
                    Email = studio.Email,
                    Role = studio.Role,
                    Token = token
                };
            }
            return null;
        }


        public FilmStudioDTO? GetFilmStudioById(int id, string? userRole, bool includeFullDetails)
        {
            var studio = _context.FilmStudios.FirstOrDefault(studio => studio.FilmStudioId == id);
            
            if (studio == null)
            {
                return null;
            }
            
            var filmStudioDTO = new FilmStudioDTO
            {
                Username = studio.Username,
                Name = studio.Name,
                City = studio.City,
                Email = studio.Email,
                Role = studio.Role
            };

            return filmStudioDTO;
        }

        // // Hämta alla filmstudios
        public List<FilmStudioDTO> GetAllFilmStudios(string? userRole, bool includeFullDetails)
        {
            var studios = _context.FilmStudios
                .Include(studio => studio.RentedFilms)
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
                    RentedFilms = studio.RentedFilms.Select(fc => new FilmCopyDTO
                    {
                        FilmCopyId = fc.FilmCopyId,
                        Title = fc.Title,
                        IsRented = fc.IsRented
                    }).ToList()
                }).ToList();
            }
            else
            {
                return studios.Select(studio => new FilmStudioDTO
                {
                    FilmStudioId = studio.FilmStudioId,
                    Name = studio.Name,
                    Username = studio.Username,
                    Role = studio.Role,
                    Email = studio.Email,
                    City = studio.City
                }).ToList();
            }
        }

        public List<FilmCopyDTO> GetRentedFilms(int studioId)
        {
            var rentedFilmIds = _context.Rentals
                .Where(r => r.StudioId == studioId)
                .Select(r => r.FilmCopyId)
                .ToList();

            if (!rentedFilmIds.Any())
            {
                return new List<FilmCopyDTO>();
            }

            var rentedFilms = _context.FilmCopies
                .Where(fc => rentedFilmIds.Contains(fc.FilmCopyId))
                .Select(fc => new FilmCopyDTO
                {
                    FilmCopyId = fc.FilmCopyId,
                    Title = fc.Title,
                    IsRented = fc.IsRented
                }).ToList();

            return rentedFilms;
        }



        public FilmCopy? GetFilmCopyById(int filmCopyId)
        {
            return _context.FilmCopies.FirstOrDefault(fc => fc.FilmCopyId == filmCopyId);
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


            public IEnumerable<FilmCopy> GetRentalsForStudio(int studioId)
            {
                return _context.FilmCopies
                    .Include(fc => fc.Film)
                    .Where(fc => fc.FilmStudioId == studioId && fc.IsRented)
                    .ToList();
            }
            internal object GetAllFilmCopies()
            {
                throw new NotImplementedException();
            }

            internal bool RentFilmToStudio(int studioId, int? filmCopyId)
            {
                throw new NotImplementedException();
            }
        }
}

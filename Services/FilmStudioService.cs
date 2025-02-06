using FilmStudioSFF.Models;
using FilmStudioSFF.Data;
using Microsoft.EntityFrameworkCore;

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
                .Include(fs => fs.RentedFilms) // För att inkludera filmkopior som hyrts ut
                .FirstOrDefault(fs => fs.FilmStudioId == id);
            return studio;
        }

        // Hämta alla filmstudios
        public List<FilmStudio> GetAllFilmStudios()
        {
            return _context.FilmStudios
                .Include(fs => fs.RentedFilms) // För att inkludera filmkopior
                .ToList();
        }

        // Hämta alla hyrda filmkopior för en specifik filmstudio
        public List<FilmCopy> GetRentedFilmCopies(int id)
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

            // Logga information om filmkopian
            var filmExists = studio.RentedFilms.Any(fc => fc.FilmCopyId == filmCopy.FilmCopyId);
            if (filmExists)
            {
                Console.WriteLine($"FilmCopy with ID {filmCopy.FilmCopyId} already rented.");
            }
            else
            {
                Console.WriteLine($"FilmCopy with ID {filmCopy.FilmCopyId} does not exist in rented films.");
            }

            // Lägg till filmkopian om den inte redan finns i hyrda filmer
            studio.RentedFilms.Add(filmCopy);
            return true;
        }
    }
}

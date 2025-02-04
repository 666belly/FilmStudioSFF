using FilmStudioSFF.Interfaces;
using FilmStudioSFF.Models;

namespace FilmStudioSFF.Services
{
    public class FilmStudioService
    {
        private readonly List<FilmStudio> _filmStudios = new List<FilmStudio>();

        //Register new filmstudio
        public FilmStudio RegisterFilmstudio(IRegisterFilmStudio registerFilmStudio)
        {
            if (_filmStudios.Any(fs => fs.Name == registerFilmStudio.Name))
            {
                throw new InvalidOperationException("A film studio with the same name already exists.");
            }

            var newStudio = new FilmStudio
            {
                FilmStudioId = _filmStudios.Count + 1,
                Name = registerFilmStudio.Name,
                City = registerFilmStudio.City,
                Username = registerFilmStudio.Username,
                RentedFilms = new List<FilmCopy>()
            };
            
            _filmStudios.Add(newStudio);
            return newStudio;
        }

        //Get speicifc filmstudio based on id
        public FilmStudio GetFilmStudioById(int id)
        {
            var studio = _filmStudios.FirstOrDefault(fs => fs.FilmStudioId == id);
            if (studio == null)
            {
                throw new KeyNotFoundException($"Film studio with ID {id} not found.");
            }
            return studio;
        }

        //Get all filmstudios
        public List<FilmStudio> GetAllFilmStudios()
        {
            return _filmStudios;
        }

        //Get all rented filmcopies for a specific filmstudio
        public List<FilmCopy> GetRentedFilmCopies(int id)
        {
            var studio = _filmStudios.FirstOrDefault(fs => fs.FilmStudioId == id);
            return studio?.RentedFilms ?? new List<FilmCopy>();
        }

        //Rent a filmcopy
        public bool RentFilmToStudio(int studioId, FilmCopy filmCopyId)
        {   
            var studio = _filmStudios.FirstOrDefault(fs => fs.FilmStudioId == studioId);
            if (studio == null) return false;

            studio.RentedFilms.Add(filmCopyId);
            return true;
        }
    }
}
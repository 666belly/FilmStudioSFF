using System.Collections.Generic;
using FilmStudioSFF.Models;
using FilmStudioSFF.Controllers;
using System.Linq;
using FilmStudioSFF.Data; 
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FilmStudioSFF.Services
{
    public class FilmService
    {
        private List<Film> _films = new List<Film>();
        private readonly FilmStudioDbContext _context;

        public FilmService(FilmStudioDbContext context)
        {
            _context = context;
        }

        //Get all films
        public List<Film> GetAllFilms()
        {
            return _films;
        }

        //Get film by id
        public Film GetFilmById(int id)
        {
            return _films.FirstOrDefault(f => f.FilmId == id)!;

        }

        //Add new film
        public Film AddFilm(Film film)
        {
            film.FilmId = _films.Count + 1;
            _films.Add(film);
            return film;
        }

        //Update a film
        public void UpdateFilm(Film film, int id)
        {
            var existingFilm = GetFilmById(film.FilmId);
            if (existingFilm != null)
            {
                existingFilm.Title = film.Title;
                existingFilm.Description = film.Description;
                existingFilm.AvailableCopies = film.AvailableCopies;
            }
        } 

        //Delete a film
        public void DeleteFilm(int id)
        {
            var film = GetFilmById(id);
            if (film != null)
            {
                _films.Remove(film);
            }
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


        // public bool ReturnFilm(int studioId, int filmId)
        // {
        //     var studio = _context.FilmStudios
        //         .Include(fs => fs.RentedFilms)
        //         .FirstOrDefault(fs => fs.FilmStudioId == studioId);

        //     if (studio == null)
        //     {
        //         Console.WriteLine($"Filmstudio med ID {studioId} hittades inte.");
        //         return false;
        //     }

        //     var filmCopy = studio.RentedFilms.FirstOrDefault(fc => fc.FilmCopyId == filmId);
        //     if (filmCopy == null)
        //     {
        //         Console.WriteLine($"Film med ID {filmId} hittades inte i hyrda filmer.");
        //         return false;
        //     }

        //     studio.RentedFilms.Remove(filmCopy);
        //     _context.SaveChanges();
        //     return true;
        // }

    }

}
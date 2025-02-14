using FilmStudioSFF.Models;
using FilmStudioSFF.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FilmStudioSFF.Services
{
    public class FilmService
    {
        private readonly FilmStudioDbContext _context;

        public FilmService(FilmStudioDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Film> GetAllFilms()
        {
            return _context.Films.Include(f => f.FilmCopies).ToList();
        }

        public Film AddFilm(Film newFilm)
        {
            if (newFilm == null)
            {
                throw new ArgumentNullException(nameof(newFilm), "Film data is required.");
            }

            _context.Films.Add(newFilm);
            _context.SaveChanges();

            return newFilm;
        }

        public int GetNewFilmId()
        {
            var existingFilmIds = _context.Films.Select(f => f.FilmId).ToList();
            int newFilmId = 1;
            while (existingFilmIds.Contains(newFilmId))
            {
                newFilmId++;
            }
            return newFilmId;
        }

        public Film GetFilmById(int id)
        {
            return _context.Films.FirstOrDefault(f => f.FilmId == id)!;
        }

        public void UpdateFilm(Film updatedFilm)
        {
            var existingFilm = _context.Films.FirstOrDefault(f => f.FilmId == updatedFilm.FilmId);

            if (existingFilm != null)
            {
                existingFilm.Title = updatedFilm.Title;
                existingFilm.Description = updatedFilm.Description;
                existingFilm.Genre = updatedFilm.Genre;
                existingFilm.Director = updatedFilm.Director;
                existingFilm.Year = updatedFilm.Year;
                existingFilm.IsAvailable = updatedFilm.IsAvailable;

                _context.SaveChanges();
            }
        }

        public void DeleteFilm(int id)
        {
            var film = _context.Films.FirstOrDefault(f => f.FilmId == id);

            if (film != null)
            {
                _context.Films.Remove(film);
                _context.SaveChanges();
            }
        }
    }
}
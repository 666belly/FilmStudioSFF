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
        private readonly List<Film> _mockFilms; // Mock database

        public FilmService(FilmStudioDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mockFilms = GetMockFilms().ToList();
        }

        public IEnumerable<Film> GetAllFilms()
        {
            var films = _context.Films.Include(f => f.FilmCopies).ToList();
            var mockFilms = GetMockFilms();

            if (mockFilms == null)
            {
                return films;
            }

            return films.Concat(mockFilms);
        }

        private IEnumerable<Film> GetMockFilms()
        {
            return new List<Film>
            {
                new Film
                {
                    FilmId = 1,
                    Title = "Mock Film 1",
                    Director = "Alan",
                    Description = "Aionaogbn",
                    Genre = "Comedy",
                    Year = 2021,
                    IsAvailable = true,
                    FilmCopies = new List<FilmCopy>()
                },
                new Film
                {
                    FilmId = 2,
                    Title = "Mock Film 2",
                    Director = "Alan",
                    Description = "Aionaogbn",
                    Genre = "Comedy",
                    Year = 2021,
                    IsAvailable = true,
                    FilmCopies = new List<FilmCopy>()
                }
            };
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
            var mockFilmIds = _mockFilms.Select(f => f.FilmId).ToList();
            var allFilmIds = existingFilmIds.Concat(mockFilmIds).ToList();

            int newFilmId = 1; 
            while (allFilmIds.Contains(newFilmId))
            {
                newFilmId++;
            }
            return newFilmId;
        }

        public Film GetFilmById(int id)
        {
            var filmFromDb = _context.Films.FirstOrDefault(f => f.FilmId == id);

            if (filmFromDb == null)
            {
                var filmFromMock = _mockFilms.FirstOrDefault(f => f.FilmId == id);
                return filmFromMock;
            }

            return filmFromDb;
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
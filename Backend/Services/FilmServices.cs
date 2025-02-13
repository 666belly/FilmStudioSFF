using System.Collections.Generic;
using FilmStudioSFF.Models;
using FilmStudioSFF.Controllers;
using System.Linq;
using FilmStudioSFF.Data;
using Microsoft.EntityFrameworkCore;

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

            // Ensure mockFilms is not null before using Concat
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

        // Add new film (to the database)
        public Film AddFilm(Film newFilm)
        {
            if (newFilm == null)
            {
                throw new ArgumentNullException(nameof(newFilm), "Film data is required.");
            }

            // Lägg till filmen i databasen
            _context.Films.Add(newFilm);
            _context.SaveChanges(); // Spara ändringarna i databasen

            return newFilm;
        }



        // Get the next available FilmId (based on existing database records)
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


        // Get all films (now from the database only)
        // public List<Film> GetAllFilms()
        // {
        //     var filmsFromDb = _context.Films.Include(f => f.FilmCopies).ToList();
        //     var allFilms = filmsFromDb.Concat(_mockFilms).ToList();

        //     return allFilms;
        // }

        // Get a specific film by its ID (from the database)
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

        // Update an existing film in the database
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

                _context.SaveChanges(); // Save the updated film in the database
            }
        }

        // Delete a film from the database
        public void DeleteFilm(int id)
        {
            var film = _context.Films.FirstOrDefault(f => f.FilmId == id);

            if (film != null)
            {
                _context.Films.Remove(film);  // Remove the film from the database
                _context.SaveChanges();       // Save changes to persist the deletion
            }
        }
    }
}

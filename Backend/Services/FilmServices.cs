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

        public FilmService(FilmStudioDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Add new film (to the database)
        public Film AddFilm(Film newFilm)
        {
            if (newFilm == null)
            {
                throw new ArgumentNullException(nameof(newFilm), "Film data is required.");
            }

            // Ensure a new unique FilmId is set if it's not already provided
            if (newFilm.FilmId == 0)
            {
                newFilm.FilmId = GetNewFilmId();
            }

            // Add new film to the database
            _context.Films.Add(newFilm);
            _context.SaveChanges(); // Save changes to the database

            return newFilm;
        }

        // Get the next available FilmId (based on existing database records)
        public int GetNewFilmId()
        {
            // This method now works with the database
            if (!_context.Films.Any())  // Check if there are no films yet in the database
            {
                return 1;  // Start from FilmId 1
            }

            return _context.Films.Max(f => f.FilmId) + 1;  // Get the next available FilmId
        }

        // Get all films (now from the database only)
        public List<Film> GetAllFilms()
        {
            // Hämta alla filmer direkt från databasen
            var filmsFromDb = _context.Films.Include(f => f.FilmCopies).ToList();

            return filmsFromDb;
        }

        // Get a specific film by its ID (from the database)
        public Film GetFilmById(int id)
        {
            return _context.Films.FirstOrDefault(f => f.FilmId == id);
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

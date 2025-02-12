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
        private readonly FilmStudioDbContext? _context;

        private readonly List<Film> _films;

        public FilmService()
        {
            // Mockdata för filmer och kopior
            _films = new List<Film>
            {
                new Film
                {
                    FilmId = 1,
                    Title = "The Matrix",
                    Genre = "Action",
                    Director = "The Wachowskis",
                    Year = 1999,
                    Description = "A hacker learns that the world he lives in is a simulation.",
                    FilmCopies = new List<FilmCopy>
                    {
                        new FilmCopy { FilmCopyId = 1, Title = "The Matrix - Copy 1", IsRented = false },
                        new FilmCopy { FilmCopyId = 2, Title = "The Matrix - Copy 2", IsRented = false }
                    }
                },
                new Film
                {
                    FilmId = 2,
                    Title = "Inception",
                    Genre = "Sci-Fi",
                    Director = "Christopher Nolan",
                    Year = 2010,
                    Description = "A thief who enters the dreams of others to steal secrets is given a task to plant an idea.",
                    FilmCopies = new List<FilmCopy>
                    {
                        new FilmCopy { FilmCopyId = 3, Title = "Inception - Copy 1", IsRented = false },
                        new FilmCopy { FilmCopyId = 4, Title = "Inception - Copy 2", IsRented = true }
                    }
                },
                new Film
                {
                    FilmId = 3,
                    Title = "The Dark Knight",
                    Genre = "Action",
                    Director = "Christopher Nolan",
                    Year = 2008,
                    Description = "Batman must face the Joker, a criminal mastermind who seeks to create chaos.",
                    FilmCopies = new List<FilmCopy>
                    {
                        new FilmCopy { FilmCopyId = 5, Title = "The Dark Knight - Copy 1", IsRented = false },
                        new FilmCopy { FilmCopyId = 6, Title = "The Dark Knight - Copy 2", IsRented = true }
                    }
                }
            };
        }
        //Get all films
        public List<Film> GetAllFilms()
        {
            return _films.Select(film => new Film
            {
                FilmId = film.FilmId,
                Title = film.Title,
                Genre = film.Genre,
                Director = film.Director,
                Year = film.Year,
                Description = film.Description,
                FilmCopies = film.FilmCopies
            }).ToList();        
            
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
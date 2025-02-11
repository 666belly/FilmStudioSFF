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

            if (_films.Count == 0)
            {
                _films.AddRange(new List<Film>
                {
                    new Film
                    {
                        FilmId = 1,
                        Title = "Inception",
                        Genre = "Sci-Fi",
                        Director = "Christopher Nolan",
                        Year = 2010,
                        Description = "A mind-bending thriller about dreams within dreams.",
                        FilmCopies = new List<FilmCopy> 
                        {
                            new FilmCopy { FilmCopyId = 1, IsRented = false, Title = "Inception" },
                            new FilmCopy { FilmCopyId = 2, IsRented = true, Title = "Inception" }
                        }
                    },
                    new Film
                    {
                        FilmId = 2,
                        Title = "The Matrix",
                        Genre = "Sci-Fi",
                        Director = "Lana Wachowski, Lilly Wachowski",
                        Year = 1999,
                        Description = "A computer hacker learns the truth about the world.",
                        FilmCopies = new List<FilmCopy>
                        {
                            new FilmCopy { FilmCopyId = 3, IsRented = false, Title = "The Matrix" }
                        }
                    },
                    new Film
                    {
                        FilmId = 3,
                        Title = "The Godfather",
                        Genre = "Crime",
                        Director = "Francis Ford Coppola",
                        Year = 1972,
                        Description = "The aging patriarch of an organized crime dynasty transfers control to his reluctant son.",
                        FilmCopies = new List<FilmCopy> 
                        {
                            new FilmCopy { FilmCopyId = 4, IsRented = true, Title = "The Godfather" }
                        }
                    }
                });
            }
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
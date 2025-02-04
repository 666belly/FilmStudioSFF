using System.Collections.Generic;
using FilmStudioSFF.Models;
using FilmStudioSFF.Controllers;
using System.Linq;

namespace FilmStudioSFF.Services
{
    public class FilmService
    {
        private List<Film> _films = new List<Film>();

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
        public void UpdateFilm(Film film)
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
    }

}
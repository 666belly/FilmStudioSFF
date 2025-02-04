using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public class FilmStudio : IFilmStudio
    {
        public int FilmStudioId { get; set; }
        public required string Name { get; set; }
        public required string City { get; set; }
        public required string Username { get; set; }
        public required List<FilmCopy> RentedFilms { get; set; }
    }

}
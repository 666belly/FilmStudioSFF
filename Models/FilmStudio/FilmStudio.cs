using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public class FilmStudio : IFilmStudio
    {
        public int FilmStudioId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Username { get; set; }
        public List<FilmCopy> RentedFilmCopies { get; set; }
    }

}
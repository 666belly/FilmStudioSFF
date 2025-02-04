using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public class Film : IFilm
    {
        public int FilmId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; } 
        public int AvailableCopies { get; set; }
        public required List<FilmCopy> FilmCopies { get; set; }

    }
}
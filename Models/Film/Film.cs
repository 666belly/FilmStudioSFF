using System.Collections.Generic;
using FilmStudioSFF.Interfaces;

namespace FilmStudioSFF.Models
{
    public class Film : IFilm
    {
        public int FilmId { get; set; }
        public required string Genre { get; set; }
        public required string Director { get; set; }
        public int Year { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; } 
        public required List<FilmCopy> FilmCopies { get; set; }

    }
}
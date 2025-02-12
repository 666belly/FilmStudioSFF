using System.Collections.Generic;
using FilmStudioSFF.Interfaces;

namespace FilmStudioSFF.Models
{
    public class Film : IFilm
    {
    public int FilmId { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public required string Director { get; set; }
    public int Year { get; set; }
    public required string Description { get; set; }
    public bool IsAvailable { get; set; }

    // Implementerar IFilm.FilmCopies och säkerställer att det är av rätt typ
    public List<FilmCopy> FilmCopies { get; set; }
    }
}
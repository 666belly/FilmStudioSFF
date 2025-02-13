using System;
using FilmStudioSFF.Models;
public class Rental
{
    public int FilmCopyId { get; set; }
    public FilmCopy FilmCopy { get; set; }

    public int StudioId { get; set; }
    public FilmStudio FilmStudio { get; set; }
}

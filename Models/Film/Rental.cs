using System;
using FilmStudioSFF.Models;
public class Rental
{
    public int Id { get; set; }
    public int FilmCopyId { get; set; } 
    public int StudioId { get; set; }
    public FilmCopy? FilmCopy { get; set; }

}
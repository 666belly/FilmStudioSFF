using Microsoft.EntityFrameworkCore;
using FilmStudioSFF.Models;
using FilmStudioSFF.Data;
using System.Linq;
using System.Collections.Generic;

public static class DataSeeder
{
    public static void Seed(FilmStudioDbContext context)
    {
        // Ensure the database is created
        context.Database.EnsureCreated();

        // Add Films
        if (!context.Films.Any())
        {
            var film1 = new Film
            {
                FilmId = 1,
                Title = "Mock Film 1",
                Director = "Alan",
                Description = "Aionaogbn",
                Genre = "Comedy",
                Year = 2021,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var film2 = new Film
            {
                FilmId = 2,
                Title = "Mock Film 2",
                Director = "Alan",
                Description = "Aionaogbn",
                Genre = "Comedy",
                Year = 2021,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var filmCopies = new List<FilmCopy>
            {
                new FilmCopy { FilmCopyId = 1, IsRented = false, Title = "Copy 1", Film = film1, FilmStudio = null },
                new FilmCopy { FilmCopyId = 2, IsRented = false, Title = "Copy 2", Film = film1, FilmStudio = null },
                new FilmCopy { FilmCopyId = 3, IsRented = false, Title = "Copy 3", Film = film2, FilmStudio = null },
                new FilmCopy { FilmCopyId = 4, IsRented = false, Title = "Copy 4", Film = film2, FilmStudio = null }
            };

            film1.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film1));
            film2.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film2));

            context.Films.AddRange(film1, film2);
            context.FilmCopies.AddRange(filmCopies);
            context.SaveChanges();
        }
    }
}
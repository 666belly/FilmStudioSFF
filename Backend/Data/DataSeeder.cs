using Microsoft.EntityFrameworkCore;
using FilmStudioSFF.Models;
using FilmStudioSFF.Data;
using System.Linq;
using System.Collections.Generic;

public static class DataSeeder
{
    public static void Seed(FilmStudioDbContext context)
    {
        
        context.Database.EnsureCreated();

        // Add list of mockfilms
        if (!context.Films.Any())
        {
            //ChatGPT generated films and informations
            var film1 = new Film
            {
                FilmId = 1,
                Title = "Midsommar",
                Director = "Ari Aster",
                Description = "A couple travels to Northern Europe to visit a rural hometown's fabled Swedish mid-summer festival. What begins as an idyllic retreat quickly devolves into an increasingly violent and bizarre competition at the hands of a pagan cult.",
                Genre = "Horror",
                Year = 2019,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var film2 = new Film
            {
                FilmId = 2,
                Title = "Get Out",
                Director = "Jordan Peele",
                Description = "A young African-American visits his white girlfriend's parents for the weekend, where his simmering uneasiness about their reception of him eventually reaches a boiling point.",
                Genre = "Horror",
                Year = 2017,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var film3 = new Film
            {
                FilmId = 3,
                Title = "The Lighthouse",
                Director = "Robert Eggers",
                Description = "Two lighthouse keepers try to maintain their sanity while living on a remote and mysterious New England island in the 1890s.",
                Genre = "Psychological Horror",
                Year = 2019,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var film4 = new Film
            {
                FilmId = 4,
                Title = "Hereditary",
                Director = "Ari Aster",
                Description = "A grieving family is haunted by tragic and disturbing occurrences after the death of their secretive grandmother.",
                Genre = "Horror",
                Year = 2018,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var film5 = new Film
            {
                FilmId = 5,
                Title = "The Witch",
                Director = "Robert Eggers",
                Description = "A family in 1630s New England is torn apart by the forces of witchcraft, black magic, and possession.",
                Genre = "Historical Horror",
                Year = 2015,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var film6 = new Film
            {
                FilmId = 6,
                Title = "Parasite",
                Director = "Bong Joon-ho",
                Description = "A poor family, the Kims, con their way into becoming the servants of a rich family, the Parks. But their easy life gets complicated when their deception is threatened with exposure.",
                Genre = "Thriller",
                Year = 2019,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var film7 = new Film
            {
                FilmId = 7,
                Title = "The Babadook",
                Director = "Jennifer Kent",
                Description = "A single mother and her child fall into a deep well of paranoia when an eerie children's book titled 'Mister Babadook' manifests in their home.",
                Genre = "Psychological Horror",
                Year = 2014,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var film8 = new Film
            {
                FilmId = 8,
                Title = "A Quiet Place",
                Director = "John Krasinski",
                Description = "In a post-apocalyptic world, a family is forced to live in silence while hiding from monsters with ultra-sensitive hearing.",
                Genre = "Sci-Fi Horror",
                Year = 2018,
                IsAvailable = true,
                FilmCopies = new List<FilmCopy>()
            };

            var filmCopies = new List<FilmCopy>
            {
                new FilmCopy { FilmCopyId = 1, IsRented = false, Title = "Copy 1", Film = film1, FilmStudio = null },
                new FilmCopy { FilmCopyId = 2, IsRented = false, Title = "Copy 2", Film = film1, FilmStudio = null },

                new FilmCopy { FilmCopyId = 3, IsRented = false, Title = "Copy 1", Film = film2, FilmStudio = null },
                new FilmCopy { FilmCopyId = 4, IsRented = false, Title = "Copy 2", Film = film2, FilmStudio = null },

                new FilmCopy { FilmCopyId = 5, IsRented = false, Title = "Copy 1", Film = film3, FilmStudio = null },
                new FilmCopy { FilmCopyId = 6, IsRented = false, Title = "Copy 2", Film = film3, FilmStudio = null },

                new FilmCopy { FilmCopyId = 7, IsRented = false, Title = "Copy 1", Film = film4, FilmStudio = null },
                new FilmCopy { FilmCopyId = 8, IsRented = false, Title = "Copy 2", Film = film4, FilmStudio = null },

                new FilmCopy { FilmCopyId = 9, IsRented = false, Title = "Copy 1", Film = film5, FilmStudio = null },
                new FilmCopy { FilmCopyId = 10, IsRented = false, Title = "Copy 2", Film = film5, FilmStudio = null },

                new FilmCopy { FilmCopyId = 11, IsRented = false, Title = "Copy 1", Film = film6, FilmStudio = null },
                new FilmCopy { FilmCopyId = 12, IsRented = false, Title = "Copy 2", Film = film6, FilmStudio = null },

                new FilmCopy { FilmCopyId = 13, IsRented = false, Title = "Copy 1", Film = film7, FilmStudio = null },
                new FilmCopy { FilmCopyId = 14, IsRented = false, Title = "Copy 2", Film = film7, FilmStudio = null },

                new FilmCopy { FilmCopyId = 15, IsRented = false, Title = "Copy 1", Film = film8, FilmStudio = null },
                new FilmCopy { FilmCopyId = 16, IsRented = false, Title = "Copy 2", Film = film8, FilmStudio = null }
            };

            // Assign copies to films
            film1.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film1));
            film2.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film2));
            film3.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film3));
            film4.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film4));
            film5.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film5));
            film6.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film6));
            film7.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film7));
            film8.FilmCopies.AddRange(filmCopies.Where(fc => fc.Film == film8));

            // Add films and copies to the database
            context.Films.AddRange(film1, film2, film3, film4, film5, film6, film7, film8);
            context.FilmCopies.AddRange(filmCopies);
            context.SaveChanges();
        }
    }
}
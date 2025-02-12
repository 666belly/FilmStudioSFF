using Microsoft.EntityFrameworkCore;
using FilmStudioSFF.Models;

namespace FilmStudioSFF.Data
{
    public class FilmStudioDbContext : DbContext
    {
        public FilmStudioDbContext(DbContextOptions<FilmStudioDbContext> options) : base(options)
        {
        }

        public DbSet<FilmStudio> FilmStudios { get; set; }
        public DbSet<FilmCopy> FilmCopies { get; set; }
        public DbSet<Rental> Rentals { get; set; }
    }

}

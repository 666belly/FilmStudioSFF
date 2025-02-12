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
        public DbSet<Film> Films { get; set; }
    }

    public class Rental
    {
        public int RentalId { get; set; }
        public int FilmCopyId { get; set; }
        public int StudioId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Navigation properties
        public FilmCopy FilmCopy { get; set; }
        public FilmStudio FilmStudio { get; set; }
    }

}

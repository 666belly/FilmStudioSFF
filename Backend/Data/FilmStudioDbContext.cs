using Microsoft.EntityFrameworkCore;
using FilmStudioSFF.Models;
using FilmStudioSFF.Data;
using FilmStudioSFF.Controllers;
using FilmStudioSFF.Services;
using FilmStudioSFF.Interfaces;


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
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships

            // Film -> FilmCopy (One-to-Many)
            modelBuilder.Entity<Film>()
                .HasMany(f => f.FilmCopies)
                .WithOne(fc => fc.Film)
                .HasForeignKey(fc => fc.FilmId);

            // FilmStudio -> FilmCopy (One-to-Many)
            modelBuilder.Entity<FilmStudio>()
                .HasMany(fs => fs.RentedFilms)
                .WithOne(fc => fc.FilmStudio)
                .HasForeignKey(fc => fc.FilmStudioId);

            // Rental -> FilmCopy (Many-to-One)
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.FilmCopy)
                .WithMany()
                .HasForeignKey(r => r.FilmCopyId);

            // Rental -> FilmStudio (Many-to-One)
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.FilmStudio)
                .WithMany()
                .HasForeignKey(r => r.StudioId);

            // Composite key for Rental
            modelBuilder.Entity<Rental>()
                .HasKey(r => new { r.FilmCopyId, r.StudioId });
        }
    }
}
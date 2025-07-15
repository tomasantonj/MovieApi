using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Movie.Data
{
    public class MovieApiContext : DbContext
    {
        public MovieApiContext (DbContextOptions<MovieApiContext> options)
            : base(options)
        {
        }

        public DbSet<Movie.Core.Domain.Models.Movie> Movies { get; set; } = default!;
        public DbSet<Movie.Core.Domain.Models.Actor> Actors { get; set; } = default!;
        public DbSet<Movie.Core.Domain.Models.MovieDetails> MovieDetails { get; set; } = default!;
        public DbSet<Movie.Core.Domain.Models.MovieReview> MovieReviews { get; set; } = default!;
        public DbSet<Movie.Core.Domain.Models.MovieActor> MovieActors { get; set; } = default!;
        public DbSet<Movie.Core.Domain.Models.Genre> Genres { get; set; } = default!;
        public DbSet<Movie.Core.Domain.Models.Director> Directors { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Property constraints 
            modelBuilder.Entity<Movie.Core.Domain.Models.Movie>()
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Movie.Core.Domain.Models.Genre>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Movie.Core.Domain.Models.Director>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Movie.Core.Domain.Models.Actor>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            modelBuilder.Entity<Movie.Core.Domain.Models.MovieReview>()
                .Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(500);

            // TODO: If we need any specific configurations for relationships, add them here but right now the default conventions are fine
        }
    }
}

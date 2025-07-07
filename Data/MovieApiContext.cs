using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data
{
    public class MovieApiContext : DbContext
    {
        public MovieApiContext (DbContextOptions<MovieApiContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;
        public DbSet<Actor> Actor { get; set; } = default!;
        public DbSet<MovieDetails> MovieDetails { get; set; } = default!;
        public DbSet<MovieReview> MovieReview { get; set; } = default!;
        public DbSet<MovieActor> MovieActor { get; set; } = default!;
        public DbSet<Genre> Genre { get; set; } = default!;
        public DbSet<Director> Director { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Property constraints 
            modelBuilder.Entity<Movie>()
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Genre>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Director>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Actor>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            modelBuilder.Entity<MovieReview>()
                .Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(500);

            // TODO: If we need any specific configurations for relationships, add them here but right now the default conventions are fine
        }
    }
}

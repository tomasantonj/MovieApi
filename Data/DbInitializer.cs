using MovieApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MovieApi.Data
{
    public static class DbInitializer
    {
        public static void Seed(MovieApiContext context)
        {
            context.Database.Migrate();

            if (context.Movie.Any())
                return; // DB has been seeded

            var actors = new List<Actor>
            {
                new Actor { Name = "Leonardo DiCaprio" },
                new Actor { Name = "Kate Winslet" },
                new Actor { Name = "Brad Pitt" },
                new Actor { Name = "Morgan Freeman" },
                new Actor { Name = "Tom Hanks" },
                new Actor { Name = "Natalie Portman" },
                new Actor { Name = "Scarlett Johansson" },
                new Actor { Name = "Samuel L. Jackson" }
            };
            context.Actor.AddRange(actors);
            context.SaveChanges();

            var movies = new List<Movie>
            {
                new Movie { Title = "Inception", Year = 2010, Genre = "Sci-Fi", Duration = 148 },
                new Movie { Title = "Titanic", Year = 1997, Genre = "Romance", Duration = 195 },
                new Movie { Title = "Fight Club", Year = 1999, Genre = "Drama", Duration = 139 },
                new Movie { Title = "The Shawshank Redemption", Year = 1994, Genre = "Drama", Duration = 142 },
                new Movie { Title = "Forrest Gump", Year = 1994, Genre = "Drama", Duration = 142 },
                new Movie { Title = "Black Swan", Year = 2010, Genre = "Thriller", Duration = 108 },
                new Movie { Title = "Lost in Translation", Year = 2003, Genre = "Drama", Duration = 102 },
                new Movie { Title = "Pulp Fiction", Year = 1994, Genre = "Crime", Duration = 154 }
            };
            context.Movie.AddRange(movies);
            context.SaveChanges();

            var movieDetails = new List<MovieDetails>
            {
                new MovieDetails { MovieId = movies[0].Id, Synopsis = "A thief who steals corporate secrets through dream-sharing technology.", Language = "English", Budget = 160000000 },
                new MovieDetails { MovieId = movies[1].Id, Synopsis = "A seventeen-year-old aristocrat falls in love with a kind but poor artist.", Language = "English", Budget = 200000000 },
                new MovieDetails { MovieId = movies[2].Id, Synopsis = "An insomniac office worker and a soap maker form an underground fight club.", Language = "English", Budget = 63000000 },
                new MovieDetails { MovieId = movies[3].Id, Synopsis

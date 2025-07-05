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
                new Actor { Name = "Brad Pitt" }
            };
            context.Actor.AddRange(actors);
            context.SaveChanges();

            var movies = new List<Movie>
            {
                new Movie { Title = "Inception", Year = 2010, Genre = "Sci-Fi", Duration = 148 },
                new Movie { Title = "Titanic", Year = 1997, Genre = "Romance", Duration = 195 }
            };
            context.Movie.AddRange(movies);
            context.SaveChanges();

            var movieDetails = new List<MovieDetails>
            {
                new MovieDetails { MovieId = movies[0].Id, Synopsis = "A thief who steals corporate secrets through dream-sharing technology.", Language = "English", Budget = 160000000 },
                new MovieDetails { MovieId = movies[1].Id, Synopsis = "A seventeen-year-old aristocrat falls in love with a kind but poor artist.", Language = "English", Budget = 200000000 }
            };
            context.MovieDetails.AddRange(movieDetails);
            context.SaveChanges();

            var movieActors = new List<MovieActor>
            {
                new MovieActor { MovieId = movies[0].Id, ActorId = actors[0].Id }, // Inception - Leonardo
                new MovieActor { MovieId = movies[1].Id, ActorId = actors[0].Id }, // Titanic - Leonardo
                new MovieActor { MovieId = movies[1].Id, ActorId = actors[1].Id }  // Titanic - Kate
            };
            context.MovieActor.AddRange(movieActors);
            context.SaveChanges();

            var reviews = new List<MovieReview>
            {
                new MovieReview { MovieId = movies[0].Id, ReviewerName = "Alice", Rating = 9, Comment = "Mind-bending!" },
                new MovieReview { MovieId = movies[1].Id, ReviewerName = "Bob", Rating = 8, Comment = "Heartbreaking." }
            };
            context.MovieReview.AddRange(reviews);
            context.SaveChanges();
        }
    }
}

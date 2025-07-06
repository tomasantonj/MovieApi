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

            var genres = new List<Genre>
            {
                new Genre { Name = "Sci-Fi" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Thriller" },
                new Genre { Name = "Crime" }
            };
            context.Genre.AddRange(genres);
            context.SaveChanges();

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
                new Movie { Title = "Inception", Year = 2010, GenreId = genres.Single(g => g.Name == "Sci-Fi").Id, Duration = 148 },
                new Movie { Title = "Titanic", Year = 1997, GenreId = genres.Single(g => g.Name == "Romance").Id, Duration = 195 },
                new Movie { Title = "Fight Club", Year = 1999, GenreId = genres.Single(g => g.Name == "Drama").Id, Duration = 139 },
                new Movie { Title = "The Shawshank Redemption", Year = 1994, GenreId = genres.Single(g => g.Name == "Drama").Id, Duration = 142 },
                new Movie { Title = "Forrest Gump", Year = 1994, GenreId = genres.Single(g => g.Name == "Drama").Id, Duration = 142 },
                new Movie { Title = "Black Swan", Year = 2010, GenreId = genres.Single(g => g.Name == "Thriller").Id, Duration = 108 },
                new Movie { Title = "Lost in Translation", Year = 2003, GenreId = genres.Single(g => g.Name == "Drama").Id, Duration = 102 },
                new Movie { Title = "Pulp Fiction", Year = 1994, GenreId = genres.Single(g => g.Name == "Crime").Id, Duration = 154 }
            };
            context.Movie.AddRange(movies);
            context.SaveChanges();

            var movieDetails = new List<MovieDetails>
            {
                new MovieDetails { MovieId = movies[0].Id, Synopsis = "A thief who steals corporate secrets through dream-sharing technology.", Language = "English", Budget = 160000000 },
                new MovieDetails { MovieId = movies[1].Id, Synopsis = "A seventeen-year-old aristocrat falls in love with a kind but poor artist.", Language = "English", Budget = 200000000 },
                new MovieDetails { MovieId = movies[2].Id, Synopsis = "An insomniac office worker and a soap maker form an underground fight club.", Language = "English", Budget = 63000000 },
                new MovieDetails { MovieId = movies[3].Id, Synopsis = "Two imprisoned men bond over a number of years, finding solace and eventual redemption.", Language = "English", Budget = 25000000 },
                new MovieDetails { MovieId = movies[4].Id, Synopsis = "The presidencies of Forrest Gump, the Vietnam War, and more through the eyes of an Alabama man.", Language = "English", Budget = 55000000 },
                new MovieDetails { MovieId = movies[5].Id, Synopsis = "A committed dancer wins the lead role in a production of Tchaikovsky's Swan Lake.", Language = "English", Budget = 13000000 },
                new MovieDetails { MovieId = movies[6].Id, Synopsis = "A faded movie star and a neglected young woman form an unlikely bond in Tokyo.", Language = "English", Budget = 4000000 },
                new MovieDetails { MovieId = movies[7].Id, Synopsis = "The lives of two mob hitmen, a boxer, and others intertwine in four tales of violence and redemption.", Language = "English", Budget = 8000000 }
            };
            context.MovieDetails.AddRange(movieDetails);
            context.SaveChanges();

            var movieActors = new List<MovieActor>
            {
                new MovieActor { MovieId = movies[0].Id, ActorId = actors[0].Id }, // Inception - Leonardo
                new MovieActor { MovieId = movies[1].Id, ActorId = actors[0].Id }, // Titanic - Leonardo
                new MovieActor { MovieId = movies[1].Id, ActorId = actors[1].Id }, // Titanic - Kate
                new MovieActor { MovieId = movies[2].Id, ActorId = actors[2].Id }, // Fight Club - Brad
                new MovieActor { MovieId = movies[3].Id, ActorId = actors[3].Id }, // Shawshank - Morgan
                new MovieActor { MovieId = movies[4].Id, ActorId = actors[4].Id }, // Forrest Gump - Tom
                new MovieActor { MovieId = movies[5].Id, ActorId = actors[5].Id }, // Black Swan - Natalie
                new MovieActor { MovieId = movies[6].Id, ActorId = actors[6].Id }, // Lost in Translation - Scarlett
                new MovieActor { MovieId = movies[7].Id, ActorId = actors[7].Id }  // Pulp Fiction - Samuel
            };
            context.MovieActor.AddRange(movieActors);
            context.SaveChanges();

            var reviews = new List<MovieReview>
            {
                new MovieReview { MovieId = movies[0].Id, ReviewerName = "Alice", Rating = 9, Comment = "Mind-bending!" },
                new MovieReview { MovieId = movies[1].Id, ReviewerName = "Bob", Rating = 8, Comment = "Heartbreaking." },
                new MovieReview { MovieId = movies[2].Id, ReviewerName = "Charlie", Rating = 9, Comment = "Intense and thought-provoking." },
                new MovieReview { MovieId = movies[3].Id, ReviewerName = "Diana", Rating = 10, Comment = "A masterpiece." },
                new MovieReview { MovieId = movies[4].Id, ReviewerName = "Eve", Rating = 8, Comment = "Inspiring and emotional." },
                new MovieReview { MovieId = movies[5].Id, ReviewerName = "Frank", Rating = 7, Comment = "Dark and beautiful." },
                new MovieReview { MovieId = movies[6].Id, ReviewerName = "Grace", Rating = 8, Comment = "Subtle and moving." },
                new MovieReview { MovieId = movies[7].Id, ReviewerName = "Hank", Rating = 9, Comment = "Iconic and stylish." }
            };
            context.MovieReview.AddRange(reviews);
            context.SaveChanges();
        }
    }
}
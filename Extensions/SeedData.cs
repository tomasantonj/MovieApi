using MovieApi.Data;
using MovieApi.Models;
using System.Linq;

namespace MovieApi.Extensions
{
    public static class SeedData
    {
        public static void Initialize(MovieApiContext context)
        {
            if (context.Movies.Any() || context.Actors.Any())
                return; // DB has been seeded

            // Seed Actors
            var actor1 = new Actor { Name = "Tom Hanks" };
            var actor2 = new Actor { Name = "Scarlett Johansson" };
            var actor3 = new Actor { Name = "Morgan Freeman" };
            context.Actors.AddRange(actor1, actor2, actor3);
            context.SaveChanges();

            // Seed Movies
            var movie1 = new Movie { Title = "Forrest Gump", Year = 1994, Genre = "Drama", Duration = 142 };
            var movie2 = new Movie { Title="Thor Ragnarok", Year = 2017, Genre = "Action", Duration = 130 };
            context.Movies.AddRange(movie1, movie2);
            context.SaveChanges();

            // Seed MovieDetails
            var details1 = new MovieDetails { MovieId = movie1.Id, Synopsis = "The story of a man with learning difficulties and the marvelous things he gets to experience", Language = "English", Budget = 55000000 };
            var details2 = new MovieDetails { MovieId = movie2.Id, Synopsis = "Thor teams up with Hulk, Valkyrie and Loki to save Asgard from Hela", Language = "English", Budget = 180000000 };
            context.MovieDetails.AddRange(details1, details2);
            context.SaveChanges();

            // Seed MovieActors (many-to-many)
            var ma1 = new MovieActor { MovieId = movie1.Id, ActorId = actor1.Id };
            var ma2 = new MovieActor { MovieId = movie2.Id, ActorId = actor2.Id };
            var ma3 = new MovieActor { MovieId = movie1.Id, ActorId = actor3.Id };
            context.MovieActors.AddRange(ma1, ma2, ma3);
            context.SaveChanges();

            // Seed MovieReviews
            var review1 = new MovieReview { MovieId = movie1.Id, ReviewerName = "Alice", Rating = 9, Comment = "Amazing movie!" };
            var review2 = new MovieReview { MovieId = movie2.Id, ReviewerName = "Bob", Rating = 7, Comment = "Fun action scenes." };
            var review3 = new MovieReview { MovieId = movie1.Id, ReviewerName = "Charlie", Rating = 8, Comment = "Great acting." };
            context.Reviews.AddRange(review1, review2, review3);
            context.SaveChanges();
        }
    }
}
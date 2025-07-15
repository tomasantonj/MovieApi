using Microsoft.EntityFrameworkCore;
using Movie.Core.Models;
using Bogus;
using System.Collections.Generic;
using System.Linq;

namespace Movie.Data
{
    public static class DbInitializer
    {
        public static void Seed(Movie.Data.MovieApiContext context)
        {
            context.Database.Migrate();

            if (context.Movies.Any())
                return; // DB has been seeded

            // Movie genres (no available movie genres in bogus)
            var movieGenreNames = new[]
            {
                "Action", "Adventure", "Animation", "Comedy", "Crime", "Documentary", "Drama", "Family", "Fantasy", "History", "Horror", "Music", "Mystery", "Romance", "Sci-Fi", "Thriller", "War", "Western"
            };
            var genreFaker = new Faker<Genre>()
                .RuleFor(g => g.Name, (f, g) => f.PickRandom(movieGenreNames));
            var genres = genreFaker.Generate(8).DistinctBy(g => g.Name).ToList();
            context.Genres.AddRange(genres);
            context.SaveChanges();

            // Directors
            var directorFaker = new Faker<Director>()
                .RuleFor(d => d.Name, (f, d) => f.Name.FullName());
            var directors = directorFaker.Generate(10);
            context.Directors.AddRange(directors);
            context.SaveChanges();

            // Actors
            var actorFaker = new Faker<Actor>()
                .RuleFor(a => a.Name, (f, a) => f.Name.FullName());
            var actors = actorFaker.Generate(30);
            context.Actors.AddRange(actors);
            context.SaveChanges();

            // Movies
            var movieFaker = new Faker<Movie.Core.Models.Movie>()
                .RuleFor(m => m.Title, (f, m) => f.Lorem.Sentence(2, 3))
                .RuleFor(m => m.Year, (f, m) => f.Date.Between(new System.DateTime(1980, 1, 1), new System.DateTime(2024, 1, 1)).Year)
                .RuleFor(m => m.Duration, (f, m) => f.Random.Int(80, 180))
                .RuleFor(m => m.GenreId, (f, m) => f.PickRandom(genres).Id)
                .RuleFor(m => m.DirectorId, (f, m) => f.PickRandom(directors).Id);
            var movies = movieFaker.Generate(40);
            context.Movies.AddRange(movies);
            context.SaveChanges();

            // MovieDetails
            var movieDetailsFaker = new Faker<MovieDetails>()
                .RuleFor(md => md.Synopsis, (f, md) => f.Lorem.Paragraph())
                .RuleFor(md => md.Language, (f, md) => f.PickRandom(new[] { "English", "Spanish", "French", "German", "Italian", "Japanese", "Mandarin" }))
                .RuleFor(md => md.Budget, (f, md) => f.Random.Int(1000000, 200000000));
            var movieDetails = movies.Select(m => {
                var details = movieDetailsFaker.Generate();
                details.MovieId = m.Id;
                return details;
            }).ToList();
            context.MovieDetails.AddRange(movieDetails);
            context.SaveChanges();

            // MovieActors (2-5 actors per movie)
            var movieActors = new List<MovieActor>();
            var rand = new System.Random();
            foreach (var movie in movies)
            {
                var actorCount = rand.Next(2, 6);
                var selectedActors = actors.OrderBy(a => rand.Next()).Take(actorCount).ToList();
                foreach (var actor in selectedActors)
                {
                    movieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actor.Id });
                }
            }
            context.MovieActors.AddRange(movieActors);
            context.SaveChanges();

            // MovieReviews (2-6 reviews per movie)
            var reviewFaker = new Faker<MovieReview>()
                .RuleFor(r => r.ReviewerName, (f, r) => f.Name.FirstName())
                .RuleFor(r => r.Rating, (f, r) => f.Random.Int(1, 10))
                .RuleFor(r => r.Comment, (f, r) => f.Lorem.Sentence(5, 10));
            var reviews = new List<MovieReview>();
            foreach (var movie in movies)
            {
                var reviewCount = rand.Next(2, 7);
                for (int i = 0; i < reviewCount; i++)
                {
                    var review = reviewFaker.Generate();
                    review.MovieId = movie.Id;
                    reviews.Add(review);
                }
            }
            context.MovieReviews.AddRange(reviews);
            context.SaveChanges();
        }
    }
}

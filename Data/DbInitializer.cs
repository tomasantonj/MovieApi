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

            var directors = new List<Director>
            {
                new Director { Name = "Christopher Nolan" },
                new Director { Name = "James Cameron" },
                new Director { Name = "David Fincher" },
                new Director { Name = "Frank Darabont" },
                new Director { Name = "Robert Zemeckis" },
                new Director { Name = "Darren Aronofsky" },
                new Director { Name = "Sofia Coppola" },
                new Director { Name = "Quentin Tarantino" }
            };
            context.Director.AddRange(directors);
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
                new Movie { Title = "Inception", Year = 2010, GenreId = genres.Single(g => g.Name == "Sci-Fi").Id, DirectorId = directors.Single(d => d.Name == "Christopher Nolan").Id, Duration = 148 },
                new Movie { Title = "Titanic", Year = 1997, GenreId = genres.Single(g => g.Name == "Romance").Id, DirectorId = directors.Single(d => d.Name == "James Cameron").Id, Duration = 195 },
                new Movie { Title = "Fight Club", Year = 1999, GenreId = genres.Single(g => g.Name == "Drama").Id, DirectorId = directors.Single(d => d.Name == "David Fincher").Id, Duration = 139 },
                new Movie { Title = "The Shawshank Redemption", Year = 1994, GenreId = genres.Single(g => g.Name == "Drama").Id, DirectorId = directors.Single(d => d.Name == "Frank Darabont").Id, Duration = 142 },
                new Movie { Title = "Forrest Gump", Year = 1994, GenreId = genres.Single(g => g.Name == "Drama").Id, DirectorId = directors.Single(d => d.Name == "Robert Zemeckis").Id, Duration = 142 },
                new Movie { Title = "Black Swan", Year = 2010, GenreId = genres.Single(g => g.Name == "Thriller").Id, DirectorId = directors.Single(d => d.Name == "Darren Aronofsky").Id, Duration = 108 },
                new Movie { Title = "Lost in Translation", Year = 2003, GenreId = genres.Single(g => g.Name == "Drama").Id, DirectorId = directors.Single(d => d.Name == "Sofia Coppola").Id, Duration = 102 },
                new Movie { Title = "Pulp Fiction", Year = 1994, GenreId = genres.Single(g => g.Name == "Crime").Id, DirectorId = directors.Single(d => d.Name == "Quentin Tarantino").Id, Duration = 154 }
            };
            context.Movie.AddRange(movies);
            context.SaveChanges();

            var movieDetails = new List<MovieDetails>
            {
                new MovieDetails { MovieId = movies[0].Id, Synopsis = "A thief who steals corporate secrets through dream-sharing technology.", Language = "English", Budget = 160000000 },
                new MovieDetails { MovieId = movies[1].Id, Synopsis = "A seventeen-year-old aristocrat falls in love with a kind but poor artist.", Language = "English", Budget = 200000000 },
                new MovieDetails { MovieId = movies[2].Id, Synopsis = "An insomniac office worker and a soap maker form an underground fight club.", Language = "English", Budget = 63000000 },
                new MovieDetails { MovieId = movies[3].Id, Synopsis = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.", Language = "English", Budget = 25000000 },
                new MovieDetails { MovieId = movies[4].Id, Synopsis = "The presidencies of Kennedy and Johnson, the Vietnam War, the Watergate scandal and other events in the life of an American", Language = "English", Budget = 55000000 },
                new MovieDetails { MovieId = movies[5].Id, Synopsis = "A committed dancer struggles to maintain her sanity after winning the lead role in a New York City production of Tchaikovsky's 'Swan Lake'.", Language = "English", Budget = 13000000 },
                new MovieDetails { MovieId = movies[6].Id, Synopsis = "A faded movie star and a neglected young girl form an unlikely bond after crossing paths in Tokyo.", Language = "English", Budget = 4000000 },
                new MovieDetails { MovieId = movies[7].Id, Synopsis = "The lives of two mob hitmen, a boxer, a gangster's wife, and a pair of diner bandits intertwine in four tales of violence and redemption.", Language = "English", Budget = 8000000 }
                };
            context.MovieDetails.AddRange(movieDetails);
            context.SaveChanges();
            var movieActors = new List<MovieActor>
            {
                new MovieActor { MovieId = movies[0].Id, ActorId = actors.Single(a => a.Name == "Leonardo DiCaprio").Id },
                new MovieActor { MovieId = movies[1].Id, ActorId = actors.Single(a => a.Name == "Kate Winslet").Id },
                new MovieActor { MovieId = movies[2].Id, ActorId = actors.Single(a => a.Name == "Brad Pitt").Id },
                new MovieActor { MovieId = movies[3].Id, ActorId = actors.Single(a => a.Name == "Morgan Freeman").Id },
                new MovieActor { MovieId = movies[4].Id, ActorId = actors.Single(a => a.Name == "Tom Hanks").Id },
                new MovieActor { MovieId = movies[5].Id, ActorId = actors.Single(a => a.Name == "Natalie Portman").Id },
                new MovieActor { MovieId = movies[6].Id, ActorId = actors.Single(a => a.Name == "Scarlett Johansson").Id },
                new MovieActor { MovieId = movies[7].Id, ActorId = actors.Single(a => a.Name == "Samuel L. Jackson").Id }
            };
            context.MovieActor.AddRange(movieActors);
            context.SaveChanges();
            var reviews = new List<MovieReview>
            {
                new MovieReview { MovieId = movies[0].Id, ReviewerName = "Alice", Rating = 5, Comment = "Mind-blowing!" },
                new MovieReview { MovieId = movies[1].Id, ReviewerName = "Bob", Rating = 4, Comment = "A classic love story." },
                new MovieReview { MovieId = movies[2].Id, ReviewerName = "Charlie", Rating = 5, Comment = "An intense experience." },
                new MovieReview { MovieId = movies[3].Id, ReviewerName = "David", Rating = 5, Comment = "A masterpiece." },
                new MovieReview { MovieId = movies[4].Id, ReviewerName = "Eve", Rating = 4, Comment = "Emotional and inspiring." },
                new MovieReview { MovieId = movies[5].Id, ReviewerName = "Frank", Rating = 5, Comment = "A psychological thriller at its best." },
                new MovieReview { MovieId = movies[6].Id, ReviewerName = "Grace", Rating = 4, Comment = "Beautifully shot and acted." },
                new MovieReview { MovieId = movies[7].Id, ReviewerName = "Heidi", Rating = 5, Comment = "A cult classic." }
            };
            context.MovieReview.AddRange(reviews);
            context.SaveChanges();
        }
    }
}
       

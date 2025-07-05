using System.Collections.Generic;

namespace MovieApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        // Navigation properties
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        // One-to-One relationship with MovieDetails
        public MovieDetails MovieDetails { get; set; }
        // Many-to-Many relationship with Actor
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    }
}

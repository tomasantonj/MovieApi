namespace Movie.Core.Models
{
    public class MovieDetails
    {
        public int Id { get; set; } // Primary key
        public int MovieId { get; set; } // Foreign key to Movie
        public Movie Movie { get; set; }
        public string Synopsis { get; set; }
        public string Language { get; set; }
        public int Budget { get; set; }
    }
}
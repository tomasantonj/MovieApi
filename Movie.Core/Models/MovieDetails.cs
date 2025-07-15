namespace Movie.Core.Models
{
    public class MovieDetails
    {
        public int Id { get; set; } // Primary key
        public int VideoMovieId { get; set; } // Foreign key to VideoMovie
        public VideoMovie VideoMovie { get; set; }
        public string Synopsis { get; set; }
        public string Language { get; set; }
        public int Budget { get; set; }
    }
}
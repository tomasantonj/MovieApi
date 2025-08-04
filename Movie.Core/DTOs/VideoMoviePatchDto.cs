using System;

namespace Movie.Core.DTOs
{
    public class VideoMoviePatchDto
    {
        // Movie fields
        public string? Title { get; set; }
        public int? Year { get; set; }
        public int? GenreId { get; set; }
        public int? DirectorId { get; set; }
        public int? Duration { get; set; }
        // MovieDetails fields
        public string? Synopsis { get; set; }
        public string? Language { get; set; }
        public int? Budget { get; set; } // Change to int?
    }
}

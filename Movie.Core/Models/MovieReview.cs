namespace Movie.Core.Models
{
    public class MovieReview
    {
        public int Id { get; set; }
        public int VideoMovieId { get; set; }
        public VideoMovie VideoMovie { get; set; }
        public string ReviewerName { get; set; }
        public int Rating { get; set; } // e.g., 1-10
        public string? Comment { get; set; }
    }
}
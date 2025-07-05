namespace MovieApi.Models
{
    public class MovieReview
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string ReviewerName { get; set; }
        public int Rating { get; set; } // e.g., 1-10
        public string? Comment { get; set; }
    }
}

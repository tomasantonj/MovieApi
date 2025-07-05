namespace MovieApi.DTOs
{
    public class ReviewDto
    {
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        // Movie attributes that are available from the review
        public string? MovieTitle { get; set; }
        public int? MovieYear { get; set; }
        public string? MovieGenre { get; set; }
        public int? MovieDuration { get; set; }
    }
}

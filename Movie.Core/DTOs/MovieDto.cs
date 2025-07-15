namespace Movie.Core.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;
        public int DirectorId { get; set; }
        public string DirectorName { get; set; } = string.Empty;
        public int Duration { get; set; }
    }
}
using System.Collections.Generic;

namespace MovieApi.DTOs
{
    public class MovieDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;
        public int DirectorId { get; set; }
        public string DirectorName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public MovieDetailsDto? MovieDetails { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new();
        public List<ActorDto> Actors { get; set; } = new();
    }

    public class MovieDetailsDto
    {
        public string Synopsis { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Budget { get; set; }
    }
}

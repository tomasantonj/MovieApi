using System.ComponentModel.DataAnnotations;

namespace Movie.Core.DTOs
{
    public class MovieCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Range(1888, 2100, ErrorMessage = "Year must be between 1888 and 2100.")]
        public int Year { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Required]
        public int DirectorId { get; set; }

        [Required]
        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
        public int Duration { get; set; }
    }
}
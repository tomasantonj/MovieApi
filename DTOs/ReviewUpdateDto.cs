using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs
{
    public class ReviewUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string ReviewerName { get; set; } = string.Empty;

        [Required]
        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10.")]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        [Required]
        public int MovieId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Movie.Core.DTOs
{
    // DTO for creating new actor
    public class ActorCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
    }
}
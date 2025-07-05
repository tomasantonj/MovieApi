using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs
{
    public class ActorCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
    }
}

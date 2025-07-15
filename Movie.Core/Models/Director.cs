namespace Movie.Core.Models
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Navigation property for related movies
        public ICollection<VideoMovie> Movies { get; set; } = new List<VideoMovie>();
    }
}
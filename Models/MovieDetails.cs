namespace MovieApi.Models
{
    public class MovieDetails
    {
        // Primary key and foreign key to Movie
        public int MovieId { get; set; } 
        public Movie Movie { get; set; }
        public string Synopsis { get; set; }    
        public string Language { get; set; }
        public decimal Budget { get; set; }
    }
}

namespace Dashboard.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public string MovieTime { get; set; } 
        public string? MainImg { get; set; }

        public List<string>? SubImages { get; set; }
        public List<Actor> Actors { get; set; }


        public int CategoryId { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; internal set; }
        public Category Category { get; internal set; }
    }
}

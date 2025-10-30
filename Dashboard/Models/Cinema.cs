namespace Dashboard.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? MainImg { get; set; }

        public List<Movie> Movies { get; set; }
    }
}

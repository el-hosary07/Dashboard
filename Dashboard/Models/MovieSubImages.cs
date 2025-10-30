using Microsoft.EntityFrameworkCore;

namespace Dashboard.Models
{
    [PrimaryKey(nameof(MovieId),nameof(Img))]
    public class MovieSubImages
    {
        public string Img { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

    }
}

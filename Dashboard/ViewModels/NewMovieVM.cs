using Dashboard.Models;

namespace Dashboard.ViewModels
{
    public class NewMovieVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Cinema> Cinemas { get; set; }
        public IEnumerable<MovieSubImages>? SubImages { get; set; }
        public Movie? Movie { get; set; }

    }
}

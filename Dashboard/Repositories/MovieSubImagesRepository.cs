using Dashboard.DataAccess;
using Dashboard.Models;
using Dashboard.Repositories.IRepositories;

namespace Dashboard.Repositories
{
    public class MovieSubImagesRepository : Repository<MovieSubImages> , IMovieSubImagesRepository
    {

        ApplicationDbContext _context;

        public MovieSubImagesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void RemoveRange(IEnumerable<MovieSubImages> movies)
        {
            _context.RemoveRange(movies);
        }
    }
}

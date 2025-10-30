using Dashboard.Models;

namespace Dashboard.Repositories.IRepositories
{
    public interface IMovieSubImagesRepository : IRepository<MovieSubImages>
    {
        void RemoveRange(IEnumerable<MovieSubImages> movies);
    }
}

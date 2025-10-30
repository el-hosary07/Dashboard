using Dashboard.DataAccess;
using Dashboard.Models;
using Dashboard.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dashboard.Repositories
{
    public class Repository<T> : IRepository<T> where T : class 
    {
        private ApplicationDbContext _context;
        private DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context) 
        {
            _context= context;
            _dbSet = _context.Set<T>();
        }


        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken=default) 
        {
           var entityCreated =await _dbSet.AddAsync(entity, cancellationToken);

            
            return entityCreated.Entity;
        }

        public async Task<IEnumerable<T?>> GetAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T?, Object>>[]? include = null,
            bool tracked = true, 
            CancellationToken cancellationToken = default)
        {
           var entities = _dbSet.AsQueryable();

            if (expression is not null)
                entities= entities.Where(expression);

            if (include is not null)
            { 
                foreach (var item in include)
                {
                    entities = entities.Include(item);
                }
            }


                if (!tracked) 
                entities = entities.AsNoTracking();

            return await entities.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, Object>>[]? include =null,
            bool tracked = true,
            CancellationToken cancellationToken = default)
        {
            return (await GetAsync(expression,include, tracked, cancellationToken)).FirstOrDefault();

        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }


        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"error {ex.Message}");
                return 0 ;
            }

        }
    }
}

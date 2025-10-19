using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Actor> Actors  { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Cinema> Cinemas  { get; set; }
        public DbSet<Category> Categories  { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog = Dashboard;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");
        
        }
    }
}

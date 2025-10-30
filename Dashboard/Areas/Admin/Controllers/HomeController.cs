using Dashboard.DataAccess;
using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class HomeController : Controller
    {
        private ApplicationDbContext _context = new();

        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var Movie = _context.Movies.AsNoTracking().AsQueryable();
            return View(Movie.Select(e => new
            {
                e.Id,
                e.MainImg,
                e.Name,
                e.Price,
                e.MovieTime,
                e.Status,
                CinemaName = e.Cinema.Name,
                CategoryName = e.Category.Name,

            }).AsEnumerable());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using Dashboard.DataAccess;
using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Areas.Admin.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var Movie = _context.Movies.AsNoTracking().AsQueryable();
            return View(Movie.Select(e=>new
            {
                e.Id,
                e.MainImg,
                e.Name,
                
            }).AsEnumerable());
        }
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Movie Movie, IFormFile img)
        {
            if (img is not null && img.Length > 0)
            {
                var imgName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//assets//TaskImages", imgName);


                using (var stream = System.IO.File.Create(imgPath))
                {
                    img.CopyTo(stream);

                }

                Movie.MainImg = imgName;
            }


            _context.Movies.Add(Movie);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Movie = _context.Movies.Find(id);
            if (Movie == null)
            {
                return NotFound();
            }

            return View(Movie);
        }
        [HttpPost]
        public IActionResult Edit(Movie Movie, IFormFile img)
        {
            var movieInDB = _context.Movies.AsNoTracking().FirstOrDefault(e => e.Id == Movie.Id);
            if (img is not null)
            {
                if (img.Length > 0)
                {
                    var imgName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//assets//TaskImages", imgName);


                    using (var stream = System.IO.File.Create(imgPath))
                    {
                        img.CopyTo(stream);

                    }

                    Movie.MainImg = imgName;
                }

            }
            else
            {
                Movie.MainImg = movieInDB.MainImg;

            }


            _context.Movies.Update(Movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Movie = _context.Movies.Find(id);
            if (Movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(Movie);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

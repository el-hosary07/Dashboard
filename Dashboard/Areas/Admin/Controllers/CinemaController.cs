using Dashboard.DataAccess;
using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Areas.Admin.Controllers
{
    public class CinemaController : Controller
    {
        private ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var Cinema = _context.Cinemas.AsNoTracking().AsQueryable();
            return View(Cinema.AsEnumerable());
        }
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Cinema Cinema, IFormFile img)
        {
            if (img is not null && img.Length > 0)
            {
                var imgName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//assets//TaskImages", imgName);


                using(var stream = System.IO.File.Create(imgPath))
                {
                    img.CopyTo(stream);

                }

                Cinema.MainImg=imgName;
            }
            

            _context.Cinemas.Add(Cinema);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Cinema = _context.Cinemas.Find(id);
            if (Cinema == null)
            {
                return NotFound();
            }

            return View(Cinema);
        }
        [HttpPost]
        public IActionResult Edit(Cinema Cinema,IFormFile img)
        {
            var cinemaInDB = _context.Cinemas.AsNoTracking().FirstOrDefault(e=>e.Id==Cinema.Id);
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

                    Cinema.MainImg = imgName;
                }

            }
            else
            {
                Cinema.MainImg = cinemaInDB.MainImg;

            }


            _context.Cinemas.Update(Cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Cinema = _context.Cinemas.Find(id);
            if (Cinema == null)
            {
                return NotFound();
            }

            _context.Cinemas.Remove(Cinema);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

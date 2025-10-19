using Dashboard.DataAccess;
using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Areas.Admin.Controllers
{
    public class ActorController : Controller
    {
        private ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var Actor = _context.Actors.AsNoTracking().AsQueryable();
            return View(Actor.AsEnumerable());
        }
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(Actor Actor, IFormFile img)
        {
            if (img is not null && img.Length > 0)
            {
                var imgName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//assets//TaskImages", imgName);


                using (var stream = System.IO.File.Create(imgPath))
                {
                    img.CopyTo(stream);

                }

                Actor.MainImg = imgName;
            }


            _context.Actors.Add(Actor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Actor = _context.Actors.Find(id);
            if (Actor == null)
            {
                return NotFound();
            }

            return View(Actor);
        }
        [HttpPost]
        public IActionResult Edit(Actor Actor, IFormFile img)
        {
            var actorInDB = _context.Actors.AsNoTracking().FirstOrDefault(e => e.Id == Actor.Id);
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

                    Actor.MainImg = imgName;
                }

            }
            else
            {
                Actor.MainImg = actorInDB.MainImg;

            }


            _context.Actors.Update(Actor);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var Actor = _context.Actors.Find(id);
            if (Actor == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(Actor);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

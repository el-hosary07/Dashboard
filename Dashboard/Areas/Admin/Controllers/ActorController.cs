using Dashboard.DataAccess;
using Dashboard.Models;
using Dashboard.Repositories;
using Dashboard.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ActorController : Controller
    {
        //private ApplicationDbContext _context = new();
        private IRepository<Actor> _actorRepository;

        ActorController(IRepository<Actor> actorRepository)
        {
            _actorRepository = actorRepository;
        }


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var category = await _actorRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            return View(category.AsEnumerable());
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> New(Actor Actor, IFormFile img, CancellationToken cancellationToken)
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


            //_context.Actors.Add(Actor);
            //_context.SaveChanges();
            await _actorRepository.CreateAsync(Actor, cancellationToken);
            await _actorRepository.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var Actor = await _actorRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);//_context.Actors.Find(id);

            if (Actor == null)
            {
                return NotFound();
            }

            return View(Actor);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Actor Actor, IFormFile img, CancellationToken cancellationToken)
        {
            var actorInDB = await _actorRepository.GetOneAsync(e => e.Id == Actor.Id, tracked: false, cancellationToken: cancellationToken);//_context.Actors.AsNoTracking().FirstOrDefault(e=>e.Id==Actor.Id);
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


            _actorRepository.Update(Actor);
            await _actorRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var Actor = await _actorRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (Actor == null)
            {
                return NotFound();
            }

            _actorRepository.Delete(Actor);
            await _actorRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}

using Dashboard.DataAccess;
using Dashboard.Models;
using Dashboard.Repositories;
using Dashboard.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE},{SD.EMPLOYEE_ROLE}")]

    public class CinemaController : Controller
    {
        //private ApplicationDbContext _context = new();
        private IRepository<Cinema> _cinemaRepository;//= new Repository<Cinema>();

        public CinemaController(IRepository<Cinema> cinemaRepository)
        {
            _cinemaRepository= cinemaRepository;
        }


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var category = await _cinemaRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            return View(category.AsEnumerable());
        }
        
        [HttpGet]
        public IActionResult New()
        {
            return View(new Cinema());
        }
        [HttpPost]
        public async Task<IActionResult> New(Cinema Cinema, IFormFile img, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError(string.Empty, "Additional Error");

                TempData["error-notification"] = "Error While Saving Category";
                Console.WriteLine("Model not valid!");
                return View(Cinema);
            }
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
            

            //_context.Cinemas.Add(Cinema);
            //_context.SaveChanges();
            await _cinemaRepository.CreateAsync(Cinema, cancellationToken);
            await _cinemaRepository.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]

        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            var Cinema = await _cinemaRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);//_context.Cinemas.Find(id);
           
            if (Cinema == null)
            {
                return NotFound();
            }

            return View(Cinema);
        }
        [HttpPost]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]

        public async Task<IActionResult> Edit(Cinema Cinema,IFormFile img, CancellationToken cancellationToken)
        {
            var cinemaInDB = await _cinemaRepository.GetOneAsync(e=>e.Id==Cinema.Id,tracked:false,cancellationToken:cancellationToken) ;//_context.Cinemas.AsNoTracking().FirstOrDefault(e=>e.Id==Cinema.Id);
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


            _cinemaRepository.Update(Cinema);
            await _cinemaRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]

        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var Cinema = await _cinemaRepository.GetOneAsync(e => e.Id  == id ,cancellationToken:cancellationToken);
            if (Cinema == null)
            {
                return NotFound();
            }

            _cinemaRepository.Delete(Cinema);
            await _cinemaRepository.CommitAsync(cancellationToken) ;

            return RedirectToAction(nameof(Index));
        }
    }
}

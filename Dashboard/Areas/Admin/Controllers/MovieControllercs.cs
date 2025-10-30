using Dashboard.DataAccess;
using Dashboard.Models;
using Dashboard.Repositories;
using Dashboard.Repositories.IRepositories;
using Dashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class MovieController : Controller
    {
        private readonly IRepository<Movie> _movieRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Cinema> _cinemaRepository;
        private readonly IMovieSubImagesRepository _subImagesRepository;


        //private ApplicationDbContext _context = new();



        public MovieController(
            IRepository<Movie> movieRepository,
            IRepository<Category> categoryRepository,
            IRepository<Cinema> cinemaRepository,
            IMovieSubImagesRepository subImagesRepository
            ) 
        {
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _cinemaRepository = cinemaRepository;
            _subImagesRepository = subImagesRepository;
        }   


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetAsync(include: [e=>e.Category,e=>e.Cinema] ,tracked: false, cancellationToken: cancellationToken);

            return View(movie.AsEnumerable());
        }
        [HttpGet]
        public async Task<IActionResult> New(CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            var cinemas = await _cinemaRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);

            return View(new NewMovieVM()
            {
                Cinemas = cinemas.AsEnumerable(),
                Categories = categories.AsEnumerable(),
            });
        }
        [HttpPost]
        public async Task<IActionResult> New(Movie Movie, IFormFile MainImg, List<IFormFile> SubImages, CancellationToken cancellationToken)
        {
            if (MainImg is not null && MainImg.Length > 0)
            {
                var imgName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//assets//TaskImages", imgName);


                using (var stream = System.IO.File.Create(imgPath))
                {
                    MainImg.CopyTo(stream);

                }

                Movie.MainImg = imgName;
            }
            var movieCreated = await _movieRepository.CreateAsync(Movie, cancellationToken: cancellationToken);
            await _movieRepository.CommitAsync(cancellationToken);

            if (SubImages is not null && SubImages.Count > 0)
            {
                foreach (var item in SubImages)
                {
                    var subImagesName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                    var subImagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\TaskSubImages", subImagesName);

                    using (var stream = System.IO.File.Create(subImagesPath))
                    {
                        item.CopyTo(stream);

                    }

                    await _subImagesRepository.CreateAsync(new()
                    {
                        Img = subImagesName,
                        MovieId = movieCreated.Id
                    },cancellationToken);
                }
                await _subImagesRepository.CommitAsync(cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetOneAsync(e=>e.Id == id, cancellationToken :cancellationToken);
            if (movie == null)
            {
                return NotFound();
            }
            


            return View(new NewMovieVM()
            {
                Movie = movie,
                Categories = await _categoryRepository.GetAsync(tracked: false, cancellationToken: cancellationToken),
                Cinemas =  await _cinemaRepository.GetAsync(tracked: false, cancellationToken: cancellationToken),
                SubImages = await _subImagesRepository.GetAsync(e => e.MovieId == id, cancellationToken: cancellationToken)
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Movie Movie, IFormFile? MainImg, List<IFormFile>? SubImages,CancellationToken cancellationToken)
        {
            var movieInDB = await _movieRepository.GetOneAsync(e => e.Id == Movie.Id, tracked: false, cancellationToken: cancellationToken);
            if (MainImg is not null)
            {
                if (MainImg.Length > 0)
                {
                    var imgName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);
                    var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot//assets//TaskImages", imgName);


                    using (var stream = System.IO.File.Create(imgPath))
                    {
                        MainImg.CopyTo(stream);

                    }

                    Movie.MainImg = imgName;
                }

            }
            else
            {
                Movie.MainImg = movieInDB.MainImg;

            }

            if (SubImages is not null)
            {
                if (SubImages.Count>0)
                {
                    var oldImages = await _subImagesRepository.GetAsync(e => e.MovieId == Movie.Id);

                    _subImagesRepository.RemoveRange(oldImages);


                    foreach (var item in SubImages)
                    {
                        var subImagesName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        var subImagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\assets\\TaskSubImages", subImagesName);

                        using (var stream = System.IO.File.Create(subImagesPath))
                        {
                            item.CopyTo(stream);

                        }

                        await _subImagesRepository.CreateAsync(new()
                        {
                            Img = subImagesName,
                            MovieId = Movie.Id,
                        } , cancellationToken:cancellationToken);
                    }
                    await _subImagesRepository.CommitAsync(cancellationToken);
                }
            }

            _movieRepository.Update(Movie);
            await _movieRepository.CommitAsync(cancellationToken) ;

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (movie is null)
            {
                return NotFound();
            }
            
             _movieRepository.Delete(movie); 
            await _movieRepository.CommitAsync (cancellationToken) ;

            return RedirectToAction(nameof(Index));
        }
    }
}

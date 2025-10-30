using Dashboard.DataAccess;
using Dashboard.Models;
using Dashboard.Repositories;
using Dashboard.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;



namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {
        // private ApplicationDbContext _context = new();
        private IRepository<Category> _categoryRepository;//= new Repository<Category>();

        public CategoryController(IRepository<Category> categoryRepository) 
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetAsync(tracked: false, cancellationToken: cancellationToken);
            return View(category.AsEnumerable());
        }
        [HttpGet]
        public IActionResult New() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> New(Category category ,CancellationToken cancellationToken) 
        {
            await _categoryRepository.CreateAsync(category, cancellationToken);
            await _categoryRepository.CommitAsync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id ,CancellationToken cancellationToken)
        {
            var category= _categoryRepository.GetOneAsync(e=>e.Id==id,cancellationToken: cancellationToken);
            if (category == null) {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost]

        public async Task<IActionResult> Edit(Category category, CancellationToken cancellationToken)
        {
            _categoryRepository.Update(category);
            await _categoryRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (category == null)
            {
                return NotFound();
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.CommitAsync(cancellationToken);

            return RedirectToAction(nameof(Index));
        }
    }
}

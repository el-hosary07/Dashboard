using Dashboard.DataAccess;
using Dashboard.Models;
using Dashboard.Repositories;
using Dashboard.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;



namespace Dashboard.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]

    public class UserController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser>  userManager)
        {
            _userManager = userManager;
        }
        public async Task<ViewResult> Index(CancellationToken cancellationToken)
        {
            var users = _userManager.Users.AsNoTracking().AsQueryable();

            return View(users.AsEnumerable());
        }
        //[HttpGet]
        //public IActionResult New()
        //{
        //    return View(new Category());
        //}
        //[HttpPost]
        //public async Task<IActionResult> New(Category category, CancellationToken cancellationToken)
        //{

        //     if(!ModelState.IsValid)
        //    {
        //        //ModelState.AddModelError(string.Empty, "Additional Error");

        //        TempData["error-notification"] = "Error While Saving Category";
        //        Console.WriteLine("Model not valid!");
        //        return View(category);
        //    }

        //    await _categoryRepository.CreateAsync(category, cancellationToken);
        //    await _categoryRepository.CommitAsync(cancellationToken);

        //    //Response.Cookies.Append("success-notification", "Add Category Successfully");
        //    TempData["success-notification"] = "Add Category Successfully";

        //    return RedirectToAction(nameof(Index));
        //}
        //[HttpGet]
        //[Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]

        //public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        //{
        //    var category = await _categoryRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(category);
        //}
        //[HttpPost]
        //[Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]

        //public async Task<IActionResult> Edit(Category category, CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(category);
        //    }

        //    _categoryRepository.Update(category);
        //    await _categoryRepository.CommitAsync(cancellationToken);

        //    return RedirectToAction(nameof(Index));
        //}
        //[Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE},{SD.ADMIN_ROLE}")]

        //public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        //{
        //    var category = await _categoryRepository.GetOneAsync(e => e.Id == id, cancellationToken: cancellationToken);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }

        //    _categoryRepository.Delete(category);
        //    await _categoryRepository.CommitAsync(cancellationToken);

        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> LockUnLock(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);   
            
            if (user is null )return NotFound();
            
            if (await _userManager.IsInRoleAsync(user, SD.SUPER_ADMIN_ROLE))
            {
                TempData["error-notification"] = "You Can not Block Super Admin Account";
            }
            else { 
                user.LockoutEnabled = !user.LockoutEnabled;

                if(!user.LockoutEnabled)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMonths(1);
                }
                else
                {
                    user.LockoutEnd = null;
                }

                await _userManager.UpdateAsync(user);         
            }
            return RedirectToAction("Index");
        }
    
    }
    
}

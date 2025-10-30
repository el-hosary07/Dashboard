using Dashboard.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dashboard.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private IRepository<ApplicationUser> _applicationUser;

        public AccountController(IRepository<ApplicationUser> applicationUser)
        {
            _applicationUser = applicationUser;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegesterVM registerVM)
        {
            var result = await _applicationUser.CreateAsync(new()
            {
                Name=registerVM.Name,
                Email=registerVM.Email,
                UserName=registerVM.UserName,
                PasswordHash=registerVM.Password
            });
            


            return RedirectToAction("Login");
        }



    }
}

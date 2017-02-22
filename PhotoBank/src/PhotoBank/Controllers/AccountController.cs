using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoBank.ViewModels;
using PhotoBank.Models;
using Microsoft.AspNetCore.Identity;

namespace PhotoBank.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            userManager = UserManager;
            signInManager = SignInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new Models.User { Email = model.Email, UserName = model.Email, Year = model.Year };
                //adding user
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //set cookie
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index, Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}

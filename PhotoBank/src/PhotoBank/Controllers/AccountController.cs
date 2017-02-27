using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhotoBank.ViewModels;
using PhotoBank.Models;
using Microsoft.AspNetCore.Identity;
using PhotoBank.Services;
using Microsoft.AspNetCore.Authorization;
using System;

namespace PhotoBank.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        public AccountController(UserManager<User> UserManager, SignInManager<User> SignInManager)
        {
            UserManager.RegisterTokenProvider("Default", new EmailTokenProvider<User>());
            userManager = UserManager;
            signInManager = SignInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {                 
                User user = new User { Email = model.Email, UserName = model.Email, Year = model.Year };
                //adding user
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    try
                    {
                        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callBackUrl = Url.Action("ConfirmEmail", "Account",
                            new { userID = user.Id, code = code },
                            protocol: HttpContext.Request.Scheme);
                        EmailService emailService = new EmailService();
                        await emailService.SendEmailAsync(
                            model.Email,
                            "Confirm your account",
                            $"Confirm registratration: <a href='{callBackUrl}'>link</a>");
                        return LocalRedirect(returnUrl);
                    }
                    catch (Exception e)
                    {
                        await userManager.DeleteAsync(user);
                        ModelState.AddModelError(string.Empty, e.Message);
                    }
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userID, string code)
        {
            if (userID == null || code == null)
            {
                return View("Error");
            }
            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
                return View("Error");
            var result = await userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (!await userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "You didn't confirm email");
                        return View(model);
                    }
                }
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }                
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

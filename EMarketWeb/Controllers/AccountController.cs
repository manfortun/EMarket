using EMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EMarketWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            SignInResult result = await TrySignInAsync(credentials);

            if (result.Succeeded)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(credentials);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        private async Task<SignInResult> TrySignInAsync(LoginCredentials credentials)
        {
            return await _signInManager.PasswordSignInAsync(
                credentials.Username,
                credentials.Password,
                isPersistent: false,
                lockoutOnFailure: false);
        }
    }
}

using EMarket.Models;
using EMarketWeb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace EMarketWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserCache _userService;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            IUserCache userService)
        {
            _signInManager = signInManager;
            _userService = userService;
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

            _userService.ClearUserCache();

            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Attempts to sign in using credentials
        /// </summary>
        /// <param name="credentials">Login credentials</param>
        /// <returns>SignInResult of the attempt</returns>
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

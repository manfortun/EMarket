using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMarketWeb.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterController(
            ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterCredentials user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            IdentityResult result = await TrySignInUserAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Category");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }

        private async Task<IdentityResult> TrySignInUserAsync(RegisterCredentials user)
        {
            IdentityUser identUser = user.ToIdentityUser();
            IdentityResult passwordResult = await _userManager.CreateAsync(identUser, user.Password);

            if (passwordResult.Succeeded)
            {
                await SignInUserAsync(identUser);
            }

            return passwordResult;
        }

        private async Task SignInUserAsync(IdentityUser user)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
    }
}

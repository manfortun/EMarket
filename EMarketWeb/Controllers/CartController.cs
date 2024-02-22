using Microsoft.AspNetCore.Identity;
using EMarket.Utility;
using Microsoft.AspNetCore.Mvc;
using EMarket.DataAccess.Data;
using EMarket.Models;

namespace EMarketWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(
            ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string userId = await _userManager.GetUserIdAsync(User);

            IEnumerable<Cart> carts = GetUserCart(userId);

            var checkoutModel = new Checkout(carts, userId);

            return View(checkoutModel);
        }

        private IEnumerable<Cart> GetUserCart(string userId)
        {
            IEnumerable<Cart> carts = _dbContext.Carts.Where(c => c.OwnerId == userId).ToList() ?? [];
            return carts;
        }
    }
}

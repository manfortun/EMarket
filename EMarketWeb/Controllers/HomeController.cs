using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EMarketWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string userId = await _userManager.GetUserIdAsync(User);
            SetCartCountOfUser(userId);

            string searchKey = GetSearchKey();

            var displayedProducts = _dbContext.Products?.ToList()
                .Where(x => SearchPredicate(x, searchKey)) ?? [];

            return View(displayedProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string searchString)
        {
            SetSearchKey(searchString);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId)
        {
            string userId = await _userManager.GetUserIdAsync(User);
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            Cart? cart = _dbContext.Carts.FirstOrDefault(x => x.OwnerId == userId && x.ProductId == productId);

            if (cart is null)
            {
                cart = new Cart
                {
                    OwnerId = userId,
                    ProductId = productId,
                };

                _dbContext.Carts.Add(cart);
            }

            cart.Quantity++;
            _dbContext.SaveChanges();

            SetCartCountOfUser(userId);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private static bool SearchPredicate(Product product, string? searchKey)
        {
            if (string.IsNullOrEmpty(searchKey)) return true;

            return product.Name.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                (product.Category?.Name?.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private void SetCartCountOfUser(string userId)
        {
            ViewData["cartcount"] = _dbContext.Carts
                .Where(user => user.OwnerId == userId)
                .Sum(cart => cart.Quantity);
        }

        private void SetSearchKey(string searchKey)
        {
            HttpContext.Session.SetString("searchKey", JsonConvert.SerializeObject(searchKey));
        }

        private string GetSearchKey()
        {
            ISession session = HttpContext.Session;
            string? value = session.GetString("searchKey") ?? string.Empty;

            return JsonConvert.DeserializeObject<string>(value) ?? string.Empty;
        }
    }
}

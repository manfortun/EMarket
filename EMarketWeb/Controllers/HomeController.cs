using EMarket.DataAccess.Data;
using EMarket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EMarketWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<Product> products = _dbContext.Products?.ToList() ?? new List<Product>();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string searchString)
        {
            IEnumerable<Product> products = _dbContext.Products?
                .ToList()
                .Where(x => SearchPredicate(x, searchString)) ?? new List<Product>();

            return View("Index", products);
        }

        private bool SearchPredicate(Product product, string searchKey)
        {
            if (string.IsNullOrEmpty(searchKey)) return true;

            return product.Name.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
                (product.Category?.Name?.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

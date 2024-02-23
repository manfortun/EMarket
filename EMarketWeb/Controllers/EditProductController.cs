using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMarketWeb.Controllers
{
    public class EditProductController : Controller
    {
        private ILogger<HomeController> _logger;
        private ApplicationDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;
        private static HashSet<int> _productCategories = default!;
        private string? _searchKey = default!;

        public EditProductController(
        ILogger<HomeController> logger,
        ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index(int itemId, bool initial = true)
        {
            Product? product = _dbContext.Products.Find(itemId);

            if (product is null)
            {
                return BadRequest();
            }

            if (initial)
            {
                _productCategories = default!;
            }

            if (_productCategories is null)
            {
                _productCategories = product
                    .GetCategoriesArray()
                    .Select(x => x.Id)
                    .ToHashSet();
            }

            SendToView(nameof(_productCategories), _productCategories.ToArray());

            ViewBag.Categories = _dbContext.Categories
                .OrderByDescending(x => _productCategories.Contains(x.Id))
                .ThenBy(x => x.Name)
                .ToList();

            return View(product);
        }

        public IActionResult Submit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product.Id);
            }

            var oldProductCategories = _dbContext.ProductCategories
                .Where(pc => pc.ProductId == product.Id)
                .ToList();
            var newProductCategories = _productCategories
                .Select(pc => new ProductCategory { ProductId = product.Id, CategoryId = pc})
                .ToList();

            _dbContext.ProductCategories.RemoveRange(oldProductCategories);
            _dbContext.ProductCategories.AddRange(newProductCategories);

            _dbContext.Update(product);
            _dbContext.SaveChanges();

            TempData["success"] = "Successfully modified product.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ModifyCategory(int itemId, int categoryId)
        {
            Product? product = _dbContext.Products.Find(itemId);

            if (product is null)
            {
                return BadRequest();
            }

            if (_productCategories is null)
            {
                return BadRequest();
            }

            ProductCategory? productCategory = product.Category.FirstOrDefault(pc => pc.CategoryId == categoryId);

            if (!_productCategories.Add(categoryId))
            {
                _productCategories.Remove(categoryId);
            }

            SendToView(nameof(_productCategories), _productCategories.ToArray());

            return RedirectToAction("Index", new { itemId = itemId, initial = false });
        }

        private void SendToView<T>(string key, T value)
        {
            TempData[key] = value;
        }
    }
}

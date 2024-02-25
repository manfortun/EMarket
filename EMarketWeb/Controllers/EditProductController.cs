using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMarketWeb.Controllers
{
    public class EditProductController : Controller
    {
        private ApplicationDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;

        public EditProductController(
        ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IActionResult Index(string jsonString)
        {
            var viewModel = jsonString.FromJson<EditProductViewModel>();

            ViewBag.Categories = _dbContext.Categories.ToList();

            return View(viewModel);
        }

        public IActionResult Submit(EditProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                string jsonString = viewModel.ToJson();
                return View("Index", new { jsonString });
            }

            Product? product = _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == viewModel.Id);

            if (product is null)
            {
                return NotFound();
            }

            UpdateProductFromViewModel(viewModel, product);
            
            if (HasNewCategories(viewModel, product, out var newCategories))
            {
                _dbContext.ProductCategories.RemoveRange(product.Category);
                _dbContext.ProductCategories.AddRange(newCategories);
            }

            _dbContext.Update(product);
            _dbContext.SaveChanges();

            TempData["success"] = "Successfully modified product.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ModifyCategory(EditProductViewModel viewModel)
        {
            if (viewModel.NumberParameter is null)
            {
                return BadRequest();
            }

            viewModel.ToggleCategory((int)viewModel.NumberParameter);

            string jsonString = viewModel.ToJson();
            return RedirectToAction("Index", new { jsonString });
        }

        private void SendToView<T>(string key, T value)
        {
            TempData[key] = value;
        }

        private void UpdateProductFromViewModel(EditProductViewModel viewModel, Product product)
        {
            product.Name = viewModel.Name;
            product.ImageSource = viewModel.ImageSource;
            product.UnitPrice = viewModel.UnitPrice;
        }

        private bool HasNewCategories(EditProductViewModel viewModel, Product product, out List<ProductCategory> newCategories)
        {
            newCategories = new List<ProductCategory>();

            var oldCategoryList = product.Category.Select(c => c.CategoryId).ToArray();
            var newCategoryList = viewModel.GetCategories();

            bool hasChanges = oldCategoryList.Except(newCategoryList).Any();

            if (hasChanges && newCategoryList?.Any() == true)
            {
                newCategories = newCategoryList.Select(c => new ProductCategory
                {
                    ProductId = viewModel.Id,
                    CategoryId = c
                }).ToList();
            }

            return hasChanges;
        }
    }
}

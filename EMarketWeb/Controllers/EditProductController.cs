using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using EMarketWeb.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMarketWeb.Controllers
{
    public class EditProductController : Controller
    {
        private readonly string[] validFileExtensions = ["jpg", "jpeg", "png"];
        private ApplicationDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;
        private readonly IImageService _imgService;

        public EditProductController(
        ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager,
        IImageService imgService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _imgService = imgService;
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
                return RedirectToAction("Index", new { jsonString });
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

            _ = _imgService.RemoveExcept([.. _dbContext.Products.Select(p => p.ImageSource)]);

            TempData["success"] = "Successfully modified product.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(EditProductViewModel viewModel)
        {
            Product? product = _dbContext.Products.Find(viewModel.Id);

            if (product is null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            TempData["success"] = "Product successfully deleted.";
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

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            if (!_imgService.IsValid(file))
            {
                TempData["error"] = "The file selected is not an image.";
                return BadRequest("Please select a correct image format.");
            }

            _imgService.Upload(file, out string finalizedFileName);

            return Content($"~/images/{finalizedFileName}");
        }

        [HttpPost]
        public IActionResult ChangeImage(EditProductViewModel viewModel)
        {
            string jsonString = viewModel.ToJson();
            return RedirectToAction("Index", new { jsonString });
        }

        /// <summary>
        /// Updates the product based on the new information from the ViewModel
        /// </summary>
        /// <param name="viewModel">The ViewModel with the updated properties</param>
        /// <param name="product">The product to update</param>
        private void UpdateProductFromViewModel(EditProductViewModel viewModel, Product product)
        {
            product.Name = viewModel.Name;
            product.ImageSource = viewModel.ImageSource;
            product.UnitPrice = viewModel.UnitPrice;
            product.Description = viewModel.Description;
            product.DateCreated = viewModel.DateCreated;
        }

        /// <summary>
        /// Checks if it is necessary to update the category records of a product in the database
        /// </summary>
        /// <param name="viewModel">The model in the view layer</param>
        /// <param name="product">The original model from the database</param>
        /// <param name="newCategories">The new category list of the product</param>
        /// <returns>True if there are changes in categories. Otherwise, false</returns>
        private bool HasNewCategories(EditProductViewModel viewModel, Product product, out List<ProductCategory> newCategories)
        {
            newCategories = new List<ProductCategory>();

            var oldCategoryList = product.Category.Select(c => c.CategoryId).ToArray();
            var newCategoryList = viewModel.GetCategories();

            bool hasChanges = oldCategoryList.Length != newCategoryList.Length ||
                oldCategoryList.Except(newCategoryList).Any();

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

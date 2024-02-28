using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using EMarketWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EMarketWeb.Controllers
{
    public class EditProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImageService _imgService;
        private static List<string> _errorMessages = new List<string>();

        public EditProductController(
        ApplicationDbContext dbContext,
        IImageService imgService)
        {
            _dbContext = dbContext;
            _imgService = imgService;
        }

        public IActionResult Index(string jsonString)
        {
            var viewModel = jsonString.FromJson<EditProductViewModel>();

            ViewBag.Categories = _dbContext.Categories.ToList();

            foreach (var e in _errorMessages)
            {
                ModelState.AddModelError(string.Empty, e);
            }
            _errorMessages.Clear();

            return View(viewModel);
        }

        public IActionResult Submit(EditProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(c => c.Errors)
                    .OfType<ModelError>();

                _errorMessages.AddRange(modelErrors.Select(c => c.ErrorMessage));

                string jsonString = viewModel.ToJson();
                return RedirectToAction("Index", new { jsonString });
            }

            // obtain the product that is being edited from the database
            Product? product = _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == viewModel.Id);

            if (product is null)
            {
                return NotFound();
            }

            // edit the properties of the product from the view
            UpdateProductFromViewModel(viewModel, product);

            // check if necessary to edit the product-category table
            if (HasNewCategories(viewModel, product, out var newCategories))
            {
                if (product.Category is not null)
                {
                    _dbContext.ProductCategories.RemoveRange(product.Category);
                }

                _dbContext.ProductCategories.AddRange(newCategories);
            }

            _dbContext.Update(product);
            _dbContext.SaveChanges();

            // remove the images from the web root that are not in use to save space
            _ = _imgService.RemoveExcept([.. _dbContext.Products.Select(p => p.ImageSource)]);

            TempData["success"] = "Successfully modified product.";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(EditProductViewModel viewModel)
        {
            // obtain the product that is being edited from the database
            Product? product = _dbContext.Products.Find(viewModel.Id);

            if (product is null)
            {
                return NotFound();
            }

            // remove the product from the database
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            TempData["success"] = "Product successfully deleted.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ModifyCategory(EditProductViewModel viewModel)
        {
            // check the parameter
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
            // check if the uploaded file is a valid image file
            if (!_imgService.IsValid(file))
            {
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
        private static void UpdateProductFromViewModel(EditProductViewModel viewModel, Product product)
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
        private static bool HasNewCategories(EditProductViewModel viewModel, Product product, out List<ProductCategory> newCategories)
        {
            newCategories = new List<ProductCategory>();

            var oldCategoryList = product.Category?.Select(c => c.CategoryId).ToArray() ?? [];
            var newCategoryList = viewModel.GetCategories();

            // check for changes
            bool hasChanges = oldCategoryList.Length != newCategoryList.Length ||
                oldCategoryList.Except(newCategoryList).Any();

            if (hasChanges)
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

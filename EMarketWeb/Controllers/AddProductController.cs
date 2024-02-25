using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using Microsoft.AspNetCore.Mvc;

namespace EMarketWeb.Controllers
{
    public class AddProductController : Controller
    {
        private ApplicationDbContext _dbContext;

        public AddProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(string jsonString)
        {
            var viewModel = string.IsNullOrEmpty(jsonString) ? new EditProductViewModel() :
                jsonString.FromJson<EditProductViewModel>();

            ViewBag.Categories = _dbContext.Categories.ToList();
            return View(viewModel);
        }

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

        public IActionResult Save(EditProductViewModel viewModel)
        {
            viewModel.DateCreated = DateTime.Now;
            viewModel.ImageSource = "~/images/no-image.jpg";

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            _dbContext.Products.Add(viewModel);
            _dbContext.SaveChanges();

            if (viewModel.GetCategories().Any())
            {
                _dbContext.ProductCategories.AddRange(viewModel.GetCategories()
                    .Select(c => new ProductCategory
                    {
                        ProductId = viewModel.Id,
                        CategoryId = c
                    }));
            }

            _dbContext.SaveChanges();

            TempData["success"] = "Product added successfully";
            return RedirectToAction("Index", "Home");
        }
    }
}

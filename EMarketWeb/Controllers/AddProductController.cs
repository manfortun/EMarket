using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using EMarketWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EMarketWeb.Controllers
{
    public class AddProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IImageService _imgService;

        public AddProductController(
            ApplicationDbContext dbContext,
            IImageService imgService)
        {
            _dbContext = dbContext;
            _imgService = imgService;
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

            _ = _imgService.RemoveExcept([.. _dbContext.Products.Select(p => p.ImageSource).ToList()]);

            TempData["success"] = "Product added successfully";
            return RedirectToAction("Index", "Home");
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
    }
}

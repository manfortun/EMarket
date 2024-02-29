using EMarket.DataAccess.Data;
using EMarket.DataAccess.Repositories;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using EMarketWeb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EMarketWeb.Controllers
{
    public class AddProductController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IImageService _imgService;
        private static readonly List<string> _errorMessages = new List<string>();

        public AddProductController(
            ApplicationDbContext dbContext,
            IImageService imgService)
        {
            _unitOfWork = new UnitOfWork(dbContext);
            _imgService = imgService;
        }

        public IActionResult Index(string jsonString)
        {
            var viewModel = string.IsNullOrEmpty(jsonString) ?
                new EditProductViewModel() :
                jsonString.FromJson<EditProductViewModel>();

            ViewBag.Categories = _unitOfWork.CategoryRepository.Get();

            foreach (var e in _errorMessages)
            {
                ModelState.AddModelError(string.Empty, e);
            }
            _errorMessages.Clear();

            return View(viewModel);
        }

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

        public IActionResult Save(EditProductViewModel viewModel)
        {
            // DateCreated is the date the product is saved
            viewModel.DateCreated = DateTime.Now;

            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(c => c.Errors)
                    .OfType<ModelError>();

                _errorMessages.AddRange(modelErrors.Select(c => c.ErrorMessage));

                string jsonString = viewModel.ToJson();
                return RedirectToAction("Index", new { jsonString });
            }

            // add and save the products first before inserting to ProductCategories (FK constraint)
            _unitOfWork.ProductRepository.Insert(viewModel);
            _unitOfWork.Save();

            // check if product has assigned category then save
            if (viewModel.GetCategoryIdsArray().Any())
            {
                IEnumerable<ProductCategory> pcToSave = viewModel
                    .GetCategoryIdsArray()
                    .Select(c => new ProductCategory
                    {
                        ProductId = viewModel.Id,
                        CategoryId = c
                    });

                foreach (ProductCategory pc in pcToSave)
                {
                    _unitOfWork.ProductCategoryRepository.Insert(pc);
                }
                _unitOfWork.Save();
            }

            // remove any unused image from the web root to save space
            IEnumerable<string> usedFiles = [.. _unitOfWork.ProductRepository.Get().Select(record => record.ImageSource)];
            _ = _imgService.RemoveExcept(usedFiles);

            TempData["success"] = "Product added successfully";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            // check if uploaded file is valid image format
            if (!_imgService.IsValid(file))
            {
                return BadRequest("Please select a correct image format.");
            }

            // upload to web root
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

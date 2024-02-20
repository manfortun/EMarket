using EMarketWeb.Data;
using EMarketWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMarketWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        List<Category> categoryList = _dbContext.Categories.ToList();

        return View(categoryList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category model)
    {
        // check if the category name is not unique
        if (!IsCategoryNameUnique(model.Name))
        {
            ModelState.AddModelError(
                key: nameof(model.Name),
                errorMessage: "This category name already exists.");
        }

        if (ModelState.IsValid)
        {
            _dbContext.Categories.Add(model);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        return View();
    }

    private bool IsCategoryNameUnique(string categoryName)
    {
        return !_dbContext.Categories.ToList().Any(x => x.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
    }
}

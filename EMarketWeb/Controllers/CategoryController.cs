using EMarket.DataAccess.Data;
using EMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;

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
        List<Category> categoryList = _dbContext.Categories.ToList() ?? [];
        return View(categoryList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (!CanSave(category))
        {
            return View();
        }

        if (TryExecuteActionToDbContext(category, _dbContext.Categories.Add))
        {
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id is null || id == 0)
        {
            return NotFound();
        }

        Category? category = _dbContext.Categories.Find(id);

        return category is null ? NotFound() : View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (!CanSave(category))
        {
            return View();
        }

        if (TryExecuteActionToDbContext(category, _dbContext.Categories.Update))
        {
            TempData["success"] = "Category edited successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (!id.HasValue || id.Value == 0)
        {
            return NotFound();
        }

        Category? category = _dbContext.Categories.Find(id);

        return category is null ? NotFound() : View(category);
    }

    [HttpPost]
    public IActionResult Delete(Category category)
    {
        if (TryExecuteActionToDbContext(category, _dbContext.Categories.Remove))
        {
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Determines if a category can be saved based on a series of rules.
    /// </summary>
    /// <param name="category">The category to check.</param>
    /// <returns>True if the category can be saved. Otherwise, false.</returns>
    private bool CanSave(Category category)
    {
        if (!IsCategoryNameUnique(category))
        {
            ModelState.AddModelError(
                key: nameof(Category.Name),
                errorMessage: "This category name already exists.");
        }

        return ModelState.IsValid;
    }

    /// <summary>
    /// Attempts to save the changes to DbContext.
    /// </summary>
    /// <param name="category">The category to perform the action to.</param>
    /// <param name="action">The action to perform.</param>
    /// <returns>True if saving is completed successfully. Otherwise, false.</returns>
    private bool TryExecuteActionToDbContext(Category category, Func<Category, EntityEntry<Category>> action)
    {
        try
        {
            action(category);
            _dbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if the provided category name is unique among existing categories, excluding the current category (if it exists).
    /// </summary>
    /// <param name="category">The category to check.</param>
    /// <returns>True if the category name is unique. Otherwise, false.</returns>
    private bool IsCategoryNameUnique(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return !_dbContext.Categories
            .Where(x => x.Id != category.Id)
            .Select(x => x.Name)
            .ToList()
            .Contains(category.Name, StringComparer.OrdinalIgnoreCase);
    }
}

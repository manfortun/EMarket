using EMarket.DataAccess.Data;
using EMarket.DataAccess.Repositories;
using EMarket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EMarketWeb.Controllers;

public class CategoryController : Controller
{
    private readonly UnitOfWork _unitOfWork;

    public CategoryController(ApplicationDbContext dbContext)
    {
        _unitOfWork = new UnitOfWork(dbContext);
    }

    public IActionResult Index()
    {
        List<Category> categoryList = [.. _unitOfWork.CategoryRepository.Get()];
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

        _unitOfWork.CategoryRepository.Insert(category);
        TempData["success"] = "Category created successfully";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(int? id)
    {
        if (id is null || id == 0)
        {
            return NotFound();
        }

        Category? category = _unitOfWork.CategoryRepository.GetById(id);

        return category is null ? NotFound() : View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
        if (!CanSave(category))
        {
            return View();
        }

        _unitOfWork.CategoryRepository.Update(category);
        _unitOfWork.Save();
        TempData["success"] = "Category edited successfully";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Delete(int? id)
    {
        if (id is null || id == 0)
        {
            return NotFound();
        }

        Category? category = _unitOfWork.CategoryRepository.GetById(id);

        return category is null ? NotFound() : View(category);
    }

    [HttpPost]
    public IActionResult Delete(Category category)
    {
        _unitOfWork.CategoryRepository.Delete(category);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");
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
    /// Checks if the provided category name is unique among existing categories, excluding the current category (if it exists).
    /// </summary>
    /// <param name="category">The category to check.</param>
    /// <returns>True if the category name is unique. Otherwise, false.</returns>
    private bool IsCategoryNameUnique(Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return !_unitOfWork.CategoryRepository
            .Get(x => x.Id != category.Id)
            .Select(x => x.Name)
            .Contains(category.Name, StringComparer.OrdinalIgnoreCase);
    }
}

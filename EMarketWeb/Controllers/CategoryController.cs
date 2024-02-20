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

        return View();
    }
}

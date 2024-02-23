using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EMarketWeb.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private static string? _searchKey = default!;
    private static HashSet<int> _categoriesInDisplay = default!;

    public HomeController(
        ILogger<HomeController> logger,
        ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        string userId = await _userManager.GetUserIdAsync(User);
        SetCartCountOfUser(userId);

        var displayedProducts = _dbContext.Products?.ToList()
            .Where(x => SearchPredicate(x, _searchKey) && _categoriesInDisplay.Intersect(x.GetCategoriesArray().Select(y => y.Id)).Any()) ?? [];

        _categoriesInDisplay ??= _dbContext.Categories.Select(c => c.Id).ToHashSet();

        TempData[nameof(_categoriesInDisplay)] = _categoriesInDisplay;
        ViewBag.Categories = _dbContext.Categories.ToList();

        return View(displayedProducts);
    }

    public IActionResult Privacy()
    {
        ClearSession();
        return View();
    }

    public IActionResult Search(string searchString)
    {
        _searchKey = searchString;

        return RedirectToAction("Index");
    }

    public IActionResult Test(int test)
    {
        if(!_categoriesInDisplay.Add(test))
        {
            _categoriesInDisplay.Remove(test);
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> AddToCart(int productId)
    {
        ClearSession();
        string userId = await _userManager.GetUserIdAsync(User);
        if (string.IsNullOrEmpty(userId)) return BadRequest();

        Cart? cart = _dbContext.Carts.FirstOrDefault(x => x.OwnerId == userId && x.ProductId == productId);

        if (cart is null)
        {
            cart = new Cart
            {
                OwnerId = userId,
                ProductId = productId,
            };

            _dbContext.Carts.Add(cart);
        }

        cart.Quantity++;
        _dbContext.SaveChanges();

        SetCartCountOfUser(userId);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool SearchPredicate(Product product, string? searchKey)
    {
        if (string.IsNullOrEmpty(searchKey)) return true;

        var categoryIds = _dbContext.ProductCategories
            .Where(pc => pc.ProductId == product.Id)
            .Select(pc => pc.CategoryId);

        var categories = _dbContext.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();

        return product.Name.Contains(searchKey, StringComparison.OrdinalIgnoreCase) ||
            categories.Any(c => c.Name.Contains(searchKey, StringComparison.OrdinalIgnoreCase));
    }

    private void SetCartCountOfUser(string userId)
    {
        ViewData["cartcount"] = _dbContext.Carts
            .Where(user => user.OwnerId == userId)
            .Sum(cart => cart.Quantity);
    }

    private void ClearSession()
    {
        HttpContext.Session.Clear();
    }

    private void RemoveSessionObject(string key)
    {
        HttpContext.Session.Remove(key);
    }

    private void SendToView<T>(string key, T value)
    {
        TempData[key] = value;
    }
}

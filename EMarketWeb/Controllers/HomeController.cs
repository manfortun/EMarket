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
    private string? _searchKey = default!;

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

        _searchKey = GetSessionObject<string>(nameof(_searchKey));

        var displayedProducts = _dbContext.Products?.ToList()
            .Where(x => SearchPredicate(x, _searchKey)) ?? [];

        return View(displayedProducts);
    }

    public IActionResult Privacy()
    {
        ClearSession();
        return View();
    }

    public IActionResult Search(string searchString)
    {
        SetSessionObject(nameof(_searchKey), searchString);

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

    private void SetSessionObject<T>(string key, T value)
    {
        HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
    }

    private T? GetSessionObject<T>(string key)
    {
        ISession session = HttpContext.Session;
        string? value = session.GetString(key) ?? string.Empty;

        return JsonConvert.DeserializeObject<T>(value);
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

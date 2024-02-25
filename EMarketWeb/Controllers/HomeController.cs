using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
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

        int[]? categoriesInDisplay = GetSessionObject<int[]>("categoriesInDisplay");

        List<Category> allCategoryList = _dbContext.Categories.ToList();
        allCategoryList.Add(CategoryExtension.GetUncategorizedCategory());

        ViewBag.Categories = allCategoryList;

        if (categoriesInDisplay is null)
        {
            categoriesInDisplay = allCategoryList.Select(c => c.Id).ToArray();
        }

        var displayedProducts = _dbContext.Products?.ToList()
            .Where(x => SearchPredicate(x, _searchKey) &&
            (categoriesInDisplay?.Intersect(x.GetCategoriesArray().Select(y => y.Id)).Any() ?? true)) ?? [];

        SetSessionObject("categoriesInDisplay", categoriesInDisplay);
        SendToView("categoriesInDisplay", categoriesInDisplay);

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

    public IActionResult OnCategoryTicked(int categoryId)
    {
        int[]? categoriesInDisplay = GetSessionObject<int[]>("categoriesInDisplay");
        List<int> tempCategoriesInDisplayList = categoriesInDisplay?.ToList() ?? [];

        if (tempCategoriesInDisplayList.Contains(categoryId))
        {
            tempCategoriesInDisplayList.Remove(categoryId);
        }
        else
        {
            tempCategoriesInDisplayList.Add(categoryId);
        }

        SetSessionObject("categoriesInDisplay", tempCategoriesInDisplayList.ToArray());

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

    public IActionResult Edit(int itemId)
    {
        Product? product = _dbContext.Products.Find(itemId);

        if (product is null)
        {
            return NotFound();
        }

        var viewModel = new EditProductViewModel
        {
            Name = product.Name,
            UnitPrice = product.UnitPrice,
            ImageSource = product.ImageSource,
            Id = itemId,
        };

        viewModel.SetCategories(product.Category.Select(c => c.CategoryId).ToArray());

        string jsonString = viewModel.ToJson();

        return RedirectToAction("Index", "EditProduct", new { jsonString });
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

    private void SendToView<T>(string key, T value)
    {
        TempData[key] = value;
    }
}

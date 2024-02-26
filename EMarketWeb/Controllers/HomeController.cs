using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;

namespace EMarketWeb.Controllers;

public class HomeController : Controller
{
    private const int NO_OF_PRODUCTS_IN_PAGE = 11;
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private static string? _searchKey = default!;
    private static int _currentPage = 1;
    private static int _noOfPages = 0;

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

        var displayableProducts = _dbContext.Products?.ToList()
            .Where(x => SearchPredicate(x, _searchKey) &&
            (categoriesInDisplay?.Intersect(x.GetCategoriesArray().Select(y => y.Id)).Any() ?? true)) ?? [];

        var displayedProducts = displayableProducts
            .Skip((_currentPage - 1) * NO_OF_PRODUCTS_IN_PAGE)
            .Take(NO_OF_PRODUCTS_IN_PAGE);

        while (!displayedProducts.Any() && _currentPage > 1)
        {
            _currentPage--;
            displayedProducts = displayableProducts
                .Skip((_currentPage - 1) * NO_OF_PRODUCTS_IN_PAGE)
                .Take(NO_OF_PRODUCTS_IN_PAGE);
        }

        SetSessionObject("categoriesInDisplay", categoriesInDisplay);
        TempData["categoriesInDisplay"] = categoriesInDisplay;
        ViewBag.NoOfPages = _noOfPages = (int)Math.Ceiling((double)displayableProducts.Count() / NO_OF_PRODUCTS_IN_PAGE);
        ViewBag.CurrentPage = _currentPage;

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

        var viewModel = EditProductViewModel.Convert(product);

        string jsonString = viewModel.ToJson();

        return RedirectToAction("Index", "EditProduct", new { jsonString });
    }

    public IActionResult AddProduct()
    {
        return RedirectToAction("Index", "AddProduct");
    }

    public IActionResult Navigate(int pageNo)
    {
        if (pageNo < 1)
        {
            pageNo = 1;
        }
        else if (pageNo > _noOfPages)
        {
            pageNo = _noOfPages;
        }

        _currentPage = pageNo;
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Checks if a product passes a search key
    /// </summary>
    /// <param name="product">Product to check</param>
    /// <param name="searchKey">Search key</param>
    /// <returns>True if the product passes the search key. Otherwise, false</returns>
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

    /// <summary>
    /// Sets the number of items in the cart in the navbar
    /// </summary>
    /// <param name="userId"></param>
    private void SetCartCountOfUser(string userId)
    {
        ViewData["cartcount"] = _dbContext.Carts
            .Where(user => user.OwnerId == userId)
            .Sum(cart => cart.Quantity);
    }

    /// <summary>
    /// Sets the object to the session data
    /// </summary>
    /// <typeparam name="T">Data type of the object to store</typeparam>
    /// <param name="key">Key of the object to store</param>
    /// <param name="value">The object to store</param>
    private void SetSessionObject<T>(string key, T value)
    {
        HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
    }

    /// <summary>
    /// Obtains an object from the session data
    /// </summary>
    /// <typeparam name="T">Type of the object to get</typeparam>
    /// <param name="key">Key of the object stored</param>
    /// <returns>The object stored from the session</returns>
    private T? GetSessionObject<T>(string key)
    {
        ISession session = HttpContext.Session;
        string? value = session.GetString(key) ?? string.Empty;

        return JsonConvert.DeserializeObject<T>(value);
    }

    /// <summary>
    /// Clears the current session
    /// </summary>
    private void ClearSession()
    {
        HttpContext.Session.Clear();
    }
}

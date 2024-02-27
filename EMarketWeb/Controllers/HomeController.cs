using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using EMarketWeb.Services;
using EMarketWeb.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EMarketWeb.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private static readonly PageInfo<Product> _pageInfo = new(noOfItemsPerPage: 11);
    private readonly CategoryFilterService _categoryFilter;
    private static string? _searchKey = default!;

    public HomeController(
        ApplicationDbContext dbContext,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IUserCache userCache)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
        _userManager = userManager;
        _categoryFilter = userCache.Get<CategoryFilterService>();
    }

    public async Task<IActionResult> Index()
    {
        bool isVerified = await IsUserVerified(User);

        if (!isVerified)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        await InitCartCountOfUserAsync(User);

        if (!_categoryFilter.IsCategoriesSet)
        {
            _categoryFilter.SetCategories(GetExtendedCategoriesList());
        }

        int[] selectedCategories = _categoryFilter.GetSelectedCategories();

        var filteredProducts = _dbContext.Products.ToList()
            .AddFilter(_searchKey)
            .AddFilter(selectedCategories);

        _pageInfo.RefreshNoOfPages(filteredProducts);

        ViewBag.PageInfo = _pageInfo;
        ViewBag.CategoryFilter = _categoryFilter;

        return View();
    }

    private async Task<bool> IsUserVerified(ClaimsPrincipal user)
    {
        string userId = await _userManager.GetUserIdAsync(user);

        IdentityUser? identityUser = await _userManager.FindByIdAsync(userId);

        return identityUser is not null;
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Search(string searchString)
    {
        _searchKey = searchString;

        return RedirectToAction("Index");
    }

    public IActionResult OnCategoryTicked(int categoryId)
    {
        _categoryFilter.Toggle(categoryId);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> AddToCart(int productId)
    {
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

        InitCartCountOfUser(userId);

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
        _pageInfo.GoToPage(pageNo);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Obtains all available categories including the 'Uncategorized' category
    /// </summary>
    /// <returns></returns>
    private List<Category> GetExtendedCategoriesList()
    {
        var categoryListFromDb = _dbContext.Categories.ToList();
        // the 'Uncategorized' category is added to the list
        categoryListFromDb.Add(CategoryExtension.GetUncategorizedCategory());

        return categoryListFromDb;
    }

    /// <summary>
    /// Sets the number of items in the cart in the navbar
    /// </summary>
    /// <param name="userId"></param>
    private void InitCartCountOfUser(string userId)
    {
        ViewData["cartcount"] = _dbContext.Carts
            .Where(user => user.OwnerId == userId)
            .Sum(cart => cart.Quantity);
    }

    /// <summary>
    /// Sets the number of items in the cart in the navbar
    /// </summary>
    /// <param name="userId"></param>
    private async Task InitCartCountOfUserAsync(ClaimsPrincipal principal)
    {
        string userId = await _userManager.GetUserIdAsync(principal);

        InitCartCountOfUser(userId);
    }
}

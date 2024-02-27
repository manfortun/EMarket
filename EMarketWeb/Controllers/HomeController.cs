using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using EMarketWeb.Services;
using EMarketWeb.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace EMarketWeb.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private static readonly PageInfo<Product> _pageInfo = new(noOfItemsPerPage: 12);
    private readonly CategoryFilterService _categoryFilter;
    private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
    private static CancellationTokenSource cts = new CancellationTokenSource();
    private static int _featuredProductIndex = 0;
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
        if (!await IsUserVerified(User))
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

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
        return Ok();
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

    public async Task<IActionResult> GetFeaturedProductView(int direction)
    {
        if (semaphoreSlim.CurrentCount == 0)
        {
            cts.Cancel();
        }

        await semaphoreSlim.WaitAsync();

        Product? product = default!;

        try
        {
            int maxCount = await _dbContext.Products.CountAsync(cts.Token);

            _featuredProductIndex += direction;

            if (_featuredProductIndex > maxCount - 1)
            {
                _featuredProductIndex = 0;
            }
            else if (_featuredProductIndex < 0)
            {
                _featuredProductIndex = maxCount - 1;
            }

            product = await _dbContext.Products.ElementAtOrDefaultAsync(_featuredProductIndex, cts.Token);
        }
        catch (Exception)
        {
            // FALLTHROUGH
        }
        finally
        {
            semaphoreSlim.Release();
        }

        return product is null ? NotFound() : PartialView("FeaturedProduct", product);
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
}

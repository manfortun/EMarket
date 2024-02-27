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
        // check if the user exists in database
        if (!await IsUserVerified(User))
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        _categoryFilter.SetCategories(GetExtendedCategoriesListFromDb(_dbContext));

        _ = GetItems();

        ViewBag.PageInfo = _pageInfo;
        ViewBag.CategoryFilter = _categoryFilter;

        return View();
    }

    public IActionResult GetItems()
    {
        // get the categories selected from the view
        int[] selectedCategories = _categoryFilter.GetSelectedCategories();

        // filter the products based on the search key and the selected categories
        var filteredProducts = _dbContext.Products.ToList()
            .AddFilter(_searchKey)
            .AddFilter(selectedCategories);

        _pageInfo.RefreshNoOfPages(filteredProducts);

        // filters returned no items
        if (_pageInfo.NoOfPages < 1)
        {
            return NoContent();
        }

        return PartialView("HomeItemsDisplay", _pageInfo);
    }

    [HttpGet]
    public IActionResult Search(string searchString)
    {
        IEnumerable<Product> filteredProducts = _pageInfo.Items.ToList().AddFilter(searchString);

        PageInfo<Product> searchInfo = new PageInfo<Product>(_pageInfo.NoOfItemsPerPage);
        searchInfo.RefreshNoOfPages(filteredProducts);

        // filter returned no items
        if (searchInfo.NoOfPages < 1)
        {
            return NoContent();
        }

        return PartialView("HomeItemsDisplay", searchInfo);
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

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest();
        }

        string? productName = _dbContext.Products.Find(productId)?.Name;

        if (productName is null)
        {
            return NotFound();
        }

        // get or create the purchase of the user of specific productId
        Purchase? purchase = GetOrCreatePurchase(_dbContext, userId, productId);

        purchase.Quantity++;
        _dbContext.SaveChanges();

        return Ok(productName);
    }

    public IActionResult Edit(int itemId)
    {
        Product? product = _dbContext.Products.Find(itemId);

        if (product is null)
        {
            return NotFound();
        }

        // convert to /EditProduct view model
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
            // cancel prior processes
            cts.Cancel();
        }

        // wait for the process to finish
        await semaphoreSlim.WaitAsync();

        Product? product = default!;

        try
        {
            // get the upperbound
            int maxCount = await _dbContext.Products.CountAsync(cts.Token) - 1;

            _featuredProductIndex += direction;

            // keep index within the range
            LoopWithinRange(0, maxCount, ref _featuredProductIndex);

            product = await _dbContext.Products.ElementAtOrDefaultAsync(_featuredProductIndex, cts.Token);
        }
        catch (Exception)
        {
            // FALLTHROUGH
        }
        finally
        {
            // release the semaphore to allow pending processes
            semaphoreSlim.Release();
        }

        return product is null ? NotFound() : PartialView("FeaturedProduct", product);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    /// <summary>
    /// Checks if the user exists in the database
    /// </summary>
    /// <param name="user">The user to evaluate.</param>
    /// <returns>True if the user exists in database. Otherwise, false.</returns>
    private async Task<bool> IsUserVerified(ClaimsPrincipal user)
    {
        string userId = await _userManager.GetUserIdAsync(user);

        IdentityUser? identityUser = await _userManager.FindByIdAsync(userId);

        return identityUser is not null;
    }

    /// <summary>
    /// Obtains all available categories including the 'Uncategorized' category
    /// </summary>
    /// <returns></returns>
    private static List<Category> GetExtendedCategoriesListFromDb(ApplicationDbContext dbContext)
    {
        var categoryListFromDb = dbContext.Categories.ToList();
        // the 'Uncategorized' category is added to the list
        categoryListFromDb.Add(CategoryExtension.GetUncategorizedCategory());

        return categoryListFromDb;
    }

    /// <summary>
    /// Bounds a value to a range. When a value is outside the range, it returns to the other end of the range in a circular fashion.
    /// </summary>
    /// <param name="firstBound">The first value the output can have.</param>
    /// <param name="secondBound">The last value the output can have.</param>
    /// <param name="value">Returns the original value if the value is within range. Otherwise, whichever bound the value is closest to.</param>
    private static void LoopWithinRange(int firstBound, int secondBound, ref int value)
    {
        if (firstBound > secondBound)
        {
            (secondBound, firstBound) = (firstBound, secondBound);
        }

        if (value > secondBound)
        {
            value = firstBound;
        }
        else if (value < firstBound)
        {
            value = secondBound;
        }
    }

    /// <summary>
    /// Attempts to get the purchase record from database. When not existing, a new record is created instead where quantity is 0.
    /// </summary>
    /// <param name="dbContext">Database</param>
    /// <param name="userId">User ID</param>
    /// <param name="productId">Product ID</param>
    /// <returns>The obtained or created purchase record.</returns>
    private static Purchase GetOrCreatePurchase(ApplicationDbContext dbContext, string userId, int productId)
    {
        Purchase? purchase = dbContext.Purchases
            .FirstOrDefault(p => p.OwnerId == userId && p.ProductId == productId);

        if (purchase is null)
        {
            purchase = new Purchase
            {
                OwnerId = userId,
                ProductId = productId,
                Quantity = 0,
            };

            dbContext.Purchases.Add(purchase);
            dbContext.SaveChanges();
        }

        return purchase;
    }
}

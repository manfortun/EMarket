using EMarket.DataAccess.Data;
using EMarket.DataAccess.Repositories;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using EMarketWeb.Services;
using EMarketWeb.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EMarketWeb.Controllers;

public class HomeController : Controller
{
    private readonly UnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private static readonly PageInfo<Product> _pageInfo = new(noOfItemsPerPage: 12);
    private readonly CategoryFilterService _categoryFilter;
    private static int _featuredProductIndex = 0;
    private static string? _searchKey = default!;

    public HomeController(
        ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager,
        IUserCache userCache)
    {
        _unitOfWork = new UnitOfWork(dbContext);
        _userManager = userManager;
        _categoryFilter = userCache.Get<CategoryFilterService>();
    }

    public IActionResult Index()
    {
        _ = GetItems();

        ViewBag.PageInfo = _pageInfo;
        ViewBag.CategoryFilter = _categoryFilter;

        return View();
    }

    public IActionResult GetItems()
    {
        // get new categories
        List<Category> allCategories = GetExtendedCategoryList(_unitOfWork.CategoryRepository);
        List<Category> newCategories = [.. allCategories.ExceptBy(_categoryFilter.Categories.Select(c => c.Id), c => c.Id)];

        _categoryFilter.SetCategories(allCategories);

        // add any new categories as selected categories
        foreach (var c in newCategories)
        {
            _categoryFilter.Add(c.Id);
        }

        // get the categories selected from the view
        HashSet<int> selectedCategories = _categoryFilter.GetSelectedCategories();

        // filter the products based on the search key and the selected categories
        var filteredProducts = _unitOfWork.ProductRepository.Get()
            .AddFilter(_searchKey)
            .AddFilter(selectedCategories.ToArray());

        _pageInfo.SetItems(filteredProducts);

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
        IEnumerable<Product> filteredProducts = _pageInfo.Items.AddFilter(searchString);

        PageInfo<Product> searchInfo = new PageInfo<Product>(_pageInfo.NoOfItemsPerPage);
        searchInfo.SetItems(filteredProducts);

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

        string? productName = _unitOfWork.ProductRepository.GetById(productId)?.Name;

        if (productName is null)
        {
            return NotFound();
        }

        // get or create the purchase of the user of specific productId
        Purchase? purchase = GetOrCreatePurchase(_unitOfWork, userId, productId);

        purchase.Quantity++;
        _unitOfWork.Save();

        return Ok(productName);
    }

    public IActionResult Edit(int itemId)
    {
        Product? product = _unitOfWork.ProductRepository.GetById(itemId);

        if (product is null)
        {
            return NotFound();
        }

        // convert to /EditProduct view model
        var viewModel = EditProductViewModel.Convert(product);

        string jsonString = viewModel.ToJson();

        return RedirectToAction("Index", "EditProduct", new { jsonString });
    }

    public IActionResult Navigate(int pageNo)
    {
        _pageInfo.ActivePage = pageNo;

        return RedirectToAction("Index");
    }

    public IActionResult GetFeaturedProductView(int direction)
    {
        _featuredProductIndex += direction;

        // get the lower and upperbounds
        int lowerBound = 0;
        int upperBound = _unitOfWork.ProductRepository.GetCount() - 1;

        // keep index within the range
        LoopWithinRange(lowerBound, upperBound, ref _featuredProductIndex);

        Product? product = _unitOfWork.ProductRepository.ElementAtOrDefault(_featuredProductIndex);

        return product is null ? NotFound() : PartialView("FeaturedProduct", product);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> IsUserVerified()
    {
        string userId = await _userManager.GetUserIdAsync(User);

        IdentityUser? identityUser = await _userManager.FindByIdAsync(userId);

        return identityUser is null ? NotFound() : Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Obtains all available categories including the <c>Uncategorized</c> category
    /// </summary>
    /// <returns></returns>
    private static List<Category> GetExtendedCategoryList(GenericRepository<Category> categoryRepository)
    {
        List<Category> categoryListFromDb = [.. categoryRepository.Get()];
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
    private static Purchase GetOrCreatePurchase(UnitOfWork unitOfWork, string userId, int productId)
    {
        Purchase? purchase = unitOfWork.PurchaseRepository
            .FirstOrDefault(p => p.OwnerId == userId && p.ProductId == productId);

        if (purchase is null)
        {
            purchase = new Purchase
            {
                OwnerId = userId,
                ProductId = productId,
                Quantity = 0,
            };

            unitOfWork.PurchaseRepository.Insert(purchase);
            unitOfWork.Save();
        }

        return purchase;
    }
}

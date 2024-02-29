using EMarket.DataAccess.Data;
using EMarket.DataAccess.Repositories;
using EMarket.Models;
using EMarket.Models.ViewModels;
using EMarket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMarketWeb.Controllers;

public class CartController : Controller
{
    private readonly UnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private static List<Purchase> _carts = default!;
    private static OrderSummaryViewModel _orderSummaryViewModel = default!;

    public CartController(
        ApplicationDbContext dbContext,
        UserManager<IdentityUser> userManager)
    {
        _unitOfWork = new UnitOfWork(dbContext);
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(Receiver receiver)
    {
        receiver.OwnerId = await _userManager.GetUserIdAsync(User);

        try
        {
            await ClearCartAsync();

            _unitOfWork.ReceiverRepository.Insert(receiver);
            _unitOfWork.Save();
            TempData["success"] = "Successfully checked out your cart!";
        }
        catch
        {
            return BadRequest();
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> HasOrders()
    {
        string? userId = await _userManager.GetUserIdAsync(User);

        if (string.IsNullOrEmpty(userId))
        {
            return NotFound();
        }

        bool hasOrders = GetUserCart(_unitOfWork.PurchaseRepository, userId).Count > 0;

        return Ok(hasOrders);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderSummary(bool editMode)
    {
        string userId = await _userManager.GetUserIdAsync(User);

        if (string.IsNullOrEmpty(userId))
        {
            return NotFound();
        }

        List<Purchase> carts = GetUserCart(_unitOfWork.PurchaseRepository, userId);

        _orderSummaryViewModel = new OrderSummaryViewModel
        {
            Purchases = carts,
            IsEditMode = editMode
        };

        return PartialView("OrderSummary", _orderSummaryViewModel);
    }

    public IActionResult ChangeCount(int cartId, int count)
    {
        Purchase? target = _orderSummaryViewModel.Purchases.Find(c => c.Id == cartId);

        if (target is null)
        {
            return NotFound();
        }

        target.Quantity = count;

        if (target.Quantity < 1)
        {
            _orderSummaryViewModel.Purchases.Remove(target);
        }

        return PartialView("OrderSummary", _orderSummaryViewModel);
    }

    public IActionResult SetCount(int cartId, int alterVal)
    {
        if (_carts is null)
        {
            return BadRequest();
        }

        Purchase? cart = _carts.Find(c => c.Id == cartId);

        if (cart is null)
        {
            return NotFound();
        }

        cart.Quantity += alterVal;

        if (cart.Quantity < 1)
        {
            _carts.Remove(cart);
        }

        ViewBag.Carts = _carts;

        return RedirectToAction("Index", new { init = false });
    }

    public async Task<IActionResult> Save()
    {
        await ClearCartAsync();

        List<Purchase> purchases = [.. _orderSummaryViewModel.Purchases
            .Select(c => new Purchase
            {
                OwnerId = c.OwnerId,
                ProductId = c.ProductId,
                Quantity = c.Quantity,
            })];

        purchases.ForEach(_unitOfWork.PurchaseRepository.Insert);
        _unitOfWork.Save();

        return Ok();
    }

    /// <summary>
    /// Get the user's cart from database
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <returns>IEnumerable<Cart> of user</returns>
    private static List<Purchase> GetUserCart(GenericRepository<Purchase> purchaseRepository, string userId)
    {
        List<Purchase> carts = [.. purchaseRepository.Get(c => c.OwnerId == userId)];
        return carts;
    }

    /// <summary>
    /// Removes the record of the cart from database
    /// </summary>
    /// <exception cref="InvalidProgramException"></exception>
    private async Task ClearCartAsync()
    {
        string? userId = await _userManager.GetUserIdAsync(User);

        if (userId == null)
        {
            return;
        }

        List<Purchase> cart = GetUserCart(_unitOfWork.PurchaseRepository, userId);

        cart.ForEach(_unitOfWork.PurchaseRepository.Delete);
        _unitOfWork.Save();
    }
}

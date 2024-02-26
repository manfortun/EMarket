using EMarket.DataAccess.Data;
using EMarket.Models;
using EMarket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMarketWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private static Checkout? _checkout = default!;
        private static Checkout? _tempCheckout = default!;
        private static List<Cart> _carts = default!;
        private static bool _isEditMode = false;

        public CartController(
            ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(bool init = true)
        {
            if (init)
            {
                _checkout = default!;
                _tempCheckout = default!;
                _carts = default!;
                _isEditMode = false;
            }

            // obtain checkout model
            _checkout ??= await GetCheckoutModelAsync();

            // obtain user cart
            if (!_isEditMode)
            {
                string? userId = await _userManager.GetUserIdAsync(User);

                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound();
                }
                _carts = GetUserCart(_dbContext, userId);
            }

            ViewBag.IsEditMode = _isEditMode;
            ViewBag.Carts = _carts;

            return View(_checkout);
        }

        public async Task<IActionResult> Checkout(Checkout checkoutWrapper)
        {
            if (!ModelState.IsValid)
            {
                _checkout = checkoutWrapper;
                return RedirectToAction("Index", new { init = false });
            }

            try
            {
                await ClearCart();
                var receiver = new Receiver
                {
                    Id = checkoutWrapper.Id,
                    OwnerId = checkoutWrapper.OwnerId,
                    FirstName = checkoutWrapper.FirstName,
                    LastName = checkoutWrapper.LastName,
                    ContactNo = checkoutWrapper.ContactNo,
                    Address = checkoutWrapper.Address,
                };

                _dbContext.Receivers.Add(receiver);
                _dbContext.SaveChanges();
                TempData["success"] = "Successfully checked out your cart!";
            }
            catch
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ToggleEditMode(Checkout checkout)
        {
            _isEditMode = !_isEditMode;
            _checkout = checkout;

            if (_isEditMode)
            {
                _tempCheckout = (Checkout)checkout.Clone();
            }
            else if (_tempCheckout is not null)
            {
                _tempCheckout.SetPurchases(_carts);
                _checkout = _tempCheckout;

                Cart[] originalCart = [.. _dbContext.Carts
                    .Where(c => c.OwnerId == checkout.OwnerId)];

                _dbContext.Carts.RemoveRange(originalCart);
                await _dbContext.SaveChangesAsync();

                foreach (var c in _carts)
                {
                    _dbContext.Carts.Add(new Cart
                    {
                        OwnerId = c.OwnerId,
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                    });
                }

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest();
            }

            return RedirectToAction("Index", new { init = false });
        }

        public IActionResult CancelEditMode()
        {
            _isEditMode = false;
            _carts = default!;
            if (_tempCheckout is not null)
            {
                _checkout = (Checkout)_tempCheckout.Clone();
            }

            return RedirectToAction("Index", new { init = false });
        }

        public IActionResult SetCount(int cartId, int alterVal)
        {
            if (_carts is null)
            {
                return BadRequest();
            }

            Cart? cart = _carts.Find(c => c.Id == cartId);

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

        /// <summary>
        /// Get the user's cart from database
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <returns>IEnumerable<Cart> of user</returns>
        private static List<Cart> GetUserCart(ApplicationDbContext dbContext, string userId)
        {
            List<Cart> carts = [.. dbContext.Carts.Where(c => c.OwnerId == userId)];
            return carts;
        }

        /// <summary>
        /// Removes the record of the cart from database
        /// </summary>
        /// <exception cref="InvalidProgramException"></exception>
        private async Task ClearCart()
        {
            Checkout? checkoutModel = await GetCheckoutModelAsync() ??
                throw new InvalidProgramException();

            int[] purchaseIds = checkoutModel.GetPurchases();

            foreach (var id in purchaseIds)
            {
                Cart? cart = _dbContext.Carts.Find(id);
                if (cart is not null) _dbContext.Carts.Remove(cart);
            }
        }

        /// <summary>
        /// Creates a Checkout model of the current logged in user
        /// </summary>
        /// <returns>Checkout model of the user</returns>
        private async Task<Checkout?> GetCheckoutModelAsync()
        {
            string? userId = await _userManager.GetUserIdAsync(User);

            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            IEnumerable<Cart> carts = GetUserCart(_dbContext, userId);

            return new Checkout(carts, userId);
        }
    }
}

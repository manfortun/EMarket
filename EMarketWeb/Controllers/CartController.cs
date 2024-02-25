﻿using EMarket.DataAccess.Data;
using EMarket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EMarketWeb.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(
            ApplicationDbContext dbContext,
            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var checkoutModel = await GetCheckoutModelAsync();

            return View(checkoutModel);
        }

        public IActionResult Checkout(Receiver receiver)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            try
            {
                ClearCart();
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

        private IEnumerable<Cart> GetUserCart(string userId)
        {
            IEnumerable<Cart> carts = _dbContext.Carts.Where(c => c.OwnerId == userId).ToList() ?? [];
            return carts;
        }

        private async void ClearCart()
        {
            Checkout? checkoutModel = await GetCheckoutModelAsync();

            if (checkoutModel is null)
            {
                throw new InvalidProgramException();
            }

            _dbContext.Carts.RemoveRange(checkoutModel.Purchases);
        }

        private async Task<Checkout?> GetCheckoutModelAsync()
        {
            IdentityUser? user = await _userManager.GetUserAsync(User);

            if (user is null)
            {
                return null;
            }

            IEnumerable<Cart> carts = GetUserCart(user.Id);

            return new Checkout(carts, user);
        }
    }
}

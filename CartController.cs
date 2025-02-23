﻿using BookShopCart.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopCart.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<IActionResult> AddItem(int bookId, int quantity = 1, int redirect = 0)
        {
            var cartCount = await _cartRepository.AddItem(bookId, quantity);
            if (redirect == 0)
                return Ok(cartCount);

            return RedirectToAction("GetUserCart");
            
        }

        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await _cartRepository.RemoveItem(bookId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepository.GetUserCart();
            return View(cart);
        }
        public async Task<IActionResult> GetCartItemCount()
        {
            var cartItem = await _cartRepository.GetCartItemCount();
            return View(cartItem);
        }

        [HttpGet]
		public IActionResult Checkout()
        {
            return View();
        }

		public async Task<IActionResult> Checkout(CheckoutModel checkout)
        {
            bool isCheckedOut = await _cartRepository.DoCheckOut(checkout);
            if (!ModelState.IsValid)
                return View(checkout);
            if (!isCheckedOut)
                return RedirectToAction("OrderFailure");
            else
				return RedirectToAction("OrderSuccess");


		}

        public IActionResult OrderSuccess()
        {
            return View();
        }

		public IActionResult OrderFailure()
		{
			return View();
		}
	}
}

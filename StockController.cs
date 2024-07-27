using BookShopCart.Constants;
using BookShopCart.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopCart.Controllers
{
    [Authorize(Roles =nameof(Roles.Admin))]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepository;

        public StockController(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<IActionResult> Index(string sTerm = "")
        {
            var stocks = await _stockRepository.GetStocks(sTerm);
            return View(stocks);
        }

        public async Task<IActionResult> ManageStock (int bookId)
        {
            var existingStock = await _stockRepository.GetStockByBookId(bookId);
            var stock = new StockDTO
            {
                BookId = bookId,
                Quantity = existingStock != null ? existingStock.Quantity : 0,
            };

            return View(stock);
        }

        [HttpPost]
        public async Task<IActionResult> ManageStock(StockDTO stock)
        {
            if (!ModelState.IsValid)
            {
                return View(stock);
            }

            try
            {
                await _stockRepository.ManageStock(stock);
                TempData["successMessage"] = "Stock is updated successfully!";
            }
            catch (Exception)
            {

                TempData["errorMessage"] = "Stock is not updated.";
            }

            return RedirectToAction("Index");
        }
    }
}

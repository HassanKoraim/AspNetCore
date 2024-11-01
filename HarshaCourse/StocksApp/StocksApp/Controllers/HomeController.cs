using Microsoft.AspNetCore.Mvc;
using StocksApp.Services;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FinnhubService _finnhubService;

        public HomeController(FinnhubService finnhubService) 
        {
           _finnhubService = finnhubService;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            await _finnhubService.GetStockPriceQuote("AAPL");
            return View();
        }
    }
}

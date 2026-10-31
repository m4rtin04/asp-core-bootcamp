using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sección_14___Configuration___HttpClient_Work_23.Models;
using Sección_14___Configuration___HttpClient_Work_23.Options;
using Sección_14___Configuration___HttpClient_Work_23.ServiceContracts;
using Sección_14___Configuration___HttpClient_Work_23.Services;
using System.Globalization;

namespace Sección_14___Configuration___HttpClient_Work_23.Controllers
{
    
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public TradeController(IOptions<TradingOptions> tradingOptions,
                           IFinnhubService finnhubService,
                           IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
        }

        [Route("/")]
        [Route("Trade/Index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string symbol = _tradingOptions.DefaultStockSymbol ?? "MSFT";

            Dictionary<string, object>? profile= await _finnhubService.GetCompanyProfile(symbol);
            Dictionary<string, object>? quote = await _finnhubService.GetStockPriceQuote(symbol);

            var stockTrade = new StockTrade() { StockSymbol = symbol };

            if(profile != null && quote != null)
            {
                stockTrade = new StockTrade
                {
                    StockSymbol = profile["ticker"].ToString(),
                    StockName = profile["name"].ToString(),
                    Price = Convert.ToDouble(quote["c"].ToString()),
                    Quantity = 0
                };
            }
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Models;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Options;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.ServiceContracts;

namespace Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Controllers
{
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;

        public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
        }

        [Route("Stocks/Explore/{stock?}")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? stock)
        {
            List<Dictionary<string, string>>? allStocks = await _finnhubService.GetStocks();
            var stocks = new List<Stock>();

            if (allStocks != null)
            {
                var top25 = (_tradingOptions.Top25PopularStocks ?? "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                stocks = allStocks
                    .Where(s => s.TryGetValue("symbol", out var symbol) && symbol != null && top25.Contains(symbol))
                    .Select(s => new Stock
                    {
                        StockSymbol = s["symbol"],
                        StockName = s.GetValueOrDefault("description")
                    })
                    .ToList();
            }

            ViewBag.Stock = stock;
            return View(stocks);
        }
    }
}
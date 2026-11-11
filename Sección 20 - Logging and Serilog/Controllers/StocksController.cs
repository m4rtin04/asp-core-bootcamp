using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sección_20___Logging_and_Serilog.Models;
using Sección_20___Logging_and_Serilog.Options;
using Sección_20___Logging_and_Serilog.ServiceContracts;

namespace Sección_20___Logging_and_Serilog.Controllers
{
    public class StocksController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, ILogger<StocksController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _logger = logger;
        }

        [Route("Stocks/Explore/{stock?}")]
        [HttpGet]
        public async Task<IActionResult> Explore(string? stock)
        {
            _logger.LogInformation("Stocks/Explore requested. Selected stock: {Stock}", stock ?? "(none)");

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

            _logger.LogDebug("Explore returned {Count} popular stocks", stocks.Count);

            return View(stocks);
        }
    }
}
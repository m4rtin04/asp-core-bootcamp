using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockMarket.Core.ServiceContracts;
using StockMarket.UI.Models;
using StockMarket.UI.Options;

namespace StockMarket.UI.Controllers;

[Route("[controller]")]
public class StocksController : Controller
{
    private readonly TradingOptions _tradingOptions;
    private readonly ILogger<StocksController> _logger;
    private readonly IFinnhubStocksService _finnhubStocksService;

    public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubStocksService finnhubStocksService, ILogger<StocksController> logger)
    {
        _tradingOptions = tradingOptions.Value;
        _finnhubStocksService = finnhubStocksService;
        _logger = logger;
    }

    [Route("Stocks/Explore/{stock?}")]
    [HttpGet]
    public async Task<IActionResult> Explore(string? stock)
    {
        _logger.LogInformation("Stocks/Explore requested. Selected stock: {Stock}", stock ?? "(none)");

        List<Dictionary<string, string>>? allStocks = await _finnhubStocksService.GetStocks();
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

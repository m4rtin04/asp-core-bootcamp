using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockMarket.Core.ServiceContracts;
using StockMarket.UI.Models;

namespace StockMarket.UI.Controllers;

[Route("[controller]")]
public class StocksController : Controller
{
    private readonly IFinnhubStocksService _finnhubStocksService;
    private readonly TradingOptions _tradingOptions;
    private readonly ILogger<StocksController> _logger;

    public StocksController(
        IFinnhubStocksService finnhubStocksService,
        IOptions<TradingOptions> tradingOptions,
        ILogger<StocksController> logger)
    {
        _finnhubStocksService = finnhubStocksService;
        _tradingOptions = tradingOptions.Value;
        _logger = logger;
    }

    [HttpGet]
    [Route("[action]/{stock?}")]
    [Route("~/[controller]/explore/{stock?}")]
    public async Task<IActionResult> Explore(string? stock)
    {
        _logger.LogInformation("Stocks/Explore requested. Selected stock: {Stock}", stock);

        var allStocks = await _finnhubStocksService.GetStocks();
        var popular = (_tradingOptions.Top25PopularStocks ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var stocks = allStocks?
            .Where(s => s.ContainsKey("symbol") && popular.Contains(s["symbol"]))
            .Select(s => new Stock { StockName = s["description"], StockSymbol = s["symbol"] })
            .ToList() ?? new List<Stock>();

        ViewBag.stock = stock;
        return View(stocks);
    }
}

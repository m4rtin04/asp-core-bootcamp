using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using StockMarket.Core.DTO;
using StockMarket.Core.ServiceContracts;
using StockMarket.UI.Filters;
using StockMarket.UI.Models;

namespace StockMarket.UI.Controllers;

[Route("[controller]")]
public class TradeController : Controller
{
    private readonly IFinnhubCompanyProfileService _companyProfileService;
    private readonly IFinnhubStockPriceQuoteService _stockPriceQuoteService;
    private readonly IStocksService _stocksService;
    private readonly TradingOptions _tradingOptions;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TradeController> _logger;

    public TradeController(
        IFinnhubCompanyProfileService companyProfileService,
        IFinnhubStockPriceQuoteService stockPriceQuoteService,
        IStocksService stocksService,
        IOptions<TradingOptions> tradingOptions,
        IConfiguration configuration,
        ILogger<TradeController> logger)
    {
        _companyProfileService = companyProfileService;
        _stockPriceQuoteService = stockPriceQuoteService;
        _stocksService = stocksService;
        _tradingOptions = tradingOptions.Value;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    [Route("[action]/{stockSymbol?}")]
    [Route("~/[controller]")]
    public async Task<IActionResult> Index(string? stockSymbol)
    {
        stockSymbol = string.IsNullOrWhiteSpace(stockSymbol) ? "MSFT" : stockSymbol;
        _logger.LogInformation("Trade/Index requested for {Symbol}", stockSymbol);

        var profile = await _companyProfileService.GetCompanyProfile(stockSymbol);
        var quote = await _stockPriceQuoteService.GetStockPriceQuote(stockSymbol);

        var model = new StockTrade
        {
            StockSymbol = stockSymbol,
            StockName = profile != null && profile.TryGetValue("name", out var name) ? name?.ToString() : null,
            Price = quote != null && quote.TryGetValue("c", out var price)
                ? Convert.ToDouble(price?.ToString(), CultureInfo.InvariantCulture)
                : 0,
            Quantity = _tradingOptions.DefaultOrderQuantity
        };

        ViewBag.FinnhubToken = _configuration["FinnhubToken"];
        return View(model);
    }

    [HttpPost]
    [Route("[action]")]
    [TypeFilter(typeof(CreateOrderActionFilter))]
    public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
    {
        orderRequest.DateAndTimeOfOrder = DateTime.Now;
        await _stocksService.CreateBuyOrder(orderRequest);
        return RedirectToAction(nameof(Orders));
    }

    [HttpPost]
    [Route("[action]")]
    [TypeFilter(typeof(CreateOrderActionFilter))]
    public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
    {
        orderRequest.DateAndTimeOfOrder = DateTime.Now;
        await _stocksService.CreateSellOrder(orderRequest);
        return RedirectToAction(nameof(Orders));
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Orders()
    {
        var model = new Orders
        {
            BuyOrders = await _stocksService.GetBuyOrders(),
            SellOrders = await _stocksService.GetSellOrders()
        };
        return View(model);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> OrdersPDF()
    {
        var model = new Orders
        {
            BuyOrders = await _stocksService.GetBuyOrders(),
            SellOrders = await _stocksService.GetSellOrders()
        };

        return new ViewAsPdf("OrdersPDF", model, ViewData)
        {
            PageMargins = new Rotativa.AspNetCore.Options.Margins { Top = 20, Right = 20, Bottom = 20, Left = 20 },
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };
    }
}

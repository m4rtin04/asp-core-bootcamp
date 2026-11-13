using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Sección_21___Filters.DTO;
using Sección_21___Filters.Filters;
using Sección_21___Filters.Models;
using Sección_21___Filters.Options;
using Sección_21___Filters.ServiceContracts;
using System.Diagnostics;

namespace Sección_21___Filters.Controllers
{
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        private readonly IStocksService _stocksService;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IOptions<TradingOptions> tradingOptions,
                           IFinnhubService finnhubService,
                           IConfiguration configuration, IStocksService stocksService, ILogger<TradeController> logger)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
            _stocksService = stocksService;
            _logger = logger;
        }

        [Route("Trade/Index/{stockSymbol?}")]
        [HttpGet]
        public async Task<IActionResult> Index(string? stockSymbol)
        {

            string symbol = stockSymbol ?? _tradingOptions.DefaultStockSymbol ?? "MSFT";
            _logger.LogInformation("Trade/Index requested for {StockSymbol}", symbol);

            Dictionary<string, object>? profile = await _finnhubService.GetCompanyProfile(symbol);
            Dictionary<string, object>? quote = await _finnhubService.GetStockPriceQuote(symbol);

            var stockTrade = new StockTrade() { StockSymbol = symbol, Quantity = _tradingOptions.DefaultOrderQuantity };

            if (profile != null && quote != null
            && profile.TryGetValue("name", out var name)
            && quote.TryGetValue("c", out var price))
            {
                stockTrade.StockName = name?.ToString();
                stockTrade.Price = Convert.ToDouble(price?.ToString(),
                    System.Globalization.CultureInfo.InvariantCulture);
            }
            if (profile == null || quote == null || !profile.ContainsKey("name"))
            {
                _logger.LogWarning("No company data found for {StockSymbol}", symbol);
            }

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }

        [Route("Trade/Orders")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrders = await _stocksService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stocksService.GetSellOrders();

            var orders = new Orders
            {
                BuyOrders = buyOrders.OrderByDescending(o => o.DateAndTimeOfOrder).ToList(),
                SellOrders = sellOrders.OrderByDescending(o => o.DateAndTimeOfOrder).ToList()
            };

            return View(orders);
        }

        [Route("Trade/BuyOrder")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))] // Se aplica el filtro de acción
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            await _stocksService.CreateBuyOrder(orderRequest);
            _logger.LogInformation("Buy order created: {Quantity} x {StockSymbol} at {Price}",
                orderRequest.Quantity, orderRequest.StockSymbol, orderRequest.Price);

            return RedirectToAction(nameof(Orders));
        }

        [Route("Trade/SellOrder")]
        [HttpPost]
        [TypeFilter(typeof(CreateOrderActionFilter))] // Se aplica el filtro de acción]
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            await _stocksService.CreateSellOrder(orderRequest);
            _logger.LogInformation("Sell order created: {Quantity} x {StockSymbol} at {Price}",
                orderRequest.Quantity, orderRequest.StockSymbol, orderRequest.Price);

            return RedirectToAction(nameof(Orders));
        }

        // Vuelve a mostrar Index con los datos recibidos y los mensajes de error
        private IActionResult IndexWithErrors(string? symbol, string? name, uint quantity, double price)
        {
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View("Index", new StockTrade
            {
                StockSymbol = symbol,
                StockName = name,
                Quantity = quantity,
                Price = price
            });
        }

        [Route("Trade/OrdersPDF")]
        [HttpGet]
        public async Task<IActionResult> OrdersPDF()
        {
            var orders = new Orders
            {
                BuyOrders = await _stocksService.GetBuyOrders(),
                SellOrders = await _stocksService.GetSellOrders()
            };

            return new ViewAsPdf("OrdersPDF", orders)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}

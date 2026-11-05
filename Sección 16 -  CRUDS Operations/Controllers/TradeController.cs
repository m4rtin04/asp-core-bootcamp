using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sección_16____CRUDS_Operations.DTO;
using Sección_16____CRUDS_Operations.Entities;
using Sección_16____CRUDS_Operations.Models;
using Sección_16____CRUDS_Operations.Options;
using Sección_16____CRUDS_Operations.ServiceContracts;
using System.Diagnostics;

namespace Sección_16____CRUDS_Operations.Controllers
{
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        private readonly IStocksService _stocksService;

        public TradeController(IOptions<TradingOptions> tradingOptions,
                           IFinnhubService finnhubService,
                           IConfiguration configuration, IStocksService stocksService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
            _stocksService = stocksService;
        }

        [Route("/")]
        [Route("Trade/Index")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            string symbol = _tradingOptions.DefaultStockSymbol ?? "MSFT";

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
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;


            ModelState.Clear();
            TryValidateModel(buyOrderRequest);

            if (!ModelState.IsValid)
            {
                return IndexWithErrors(buyOrderRequest.StockSymbol, buyOrderRequest.StockName,
                                       buyOrderRequest.Quantity, buyOrderRequest.Price);
            }

            await _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("Trade/SellOrder")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(sellOrderRequest);

            if (!ModelState.IsValid)
            {
                return IndexWithErrors(sellOrderRequest.StockSymbol, sellOrderRequest.StockName,
                                       sellOrderRequest.Quantity, sellOrderRequest.Price);
            }

            await _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        // Vuelve a mostrar Index con los datos recibidos y los mensajes de error
        private IActionResult IndexWithErrors(string? symbol, string? name, uint quantity, double price)
        {
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View("Index", new StockTrade
            {
                StockSymbol = symbol,
                StockName = name,
                Quantity = quantity,
                Price = price
            });
        }
    }
}

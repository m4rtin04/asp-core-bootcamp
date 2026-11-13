using Microsoft.AspNetCore.Mvc;
using Sección_21___Filters.Models;
using Sección_21___Filters.ServiceContracts;
using System.Globalization;

namespace Sección_21___Filters.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubService _finnhubService;

        public SelectedStockViewComponent(IFinnhubService finnhubService)
            => _finnhubService = finnhubService;

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            if (string.IsNullOrWhiteSpace(stockSymbol))
                return Content(string.Empty);

            var profile = await _finnhubService.GetCompanyProfile(stockSymbol);
            var quote = await _finnhubService.GetStockPriceQuote(stockSymbol);

            // Si el símbolo no existe, Finnhub responde {} y no hay nada que mostrar
            if (profile == null || quote == null
                || !profile.TryGetValue("name", out var name)
                || !quote.TryGetValue("c", out var price))
                return Content(string.Empty);

            profile.TryGetValue("logo", out var logo);
            profile.TryGetValue("finnhubIndustry", out var industry);
            profile.TryGetValue("exchange", out var exchange);

            return View(new StockDetails
            {
                StockSymbol = stockSymbol,
                StockName = name?.ToString(),
                Logo = logo?.ToString(),
                Industry = industry?.ToString(),
                Exchange = exchange?.ToString(),
                Price = Convert.ToDouble(price?.ToString(), CultureInfo.InvariantCulture)
            });
        }
    }
}

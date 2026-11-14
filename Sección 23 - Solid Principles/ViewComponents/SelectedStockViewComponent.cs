using Microsoft.AspNetCore.Mvc;
using Sección_23___Solid_Principles.Models;
using Sección_23___Solid_Principles.ServiceContracts;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Sección_23___Solid_Principles.ViewComponents
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubCompanyProfileService _companyProfileService;
        private readonly IFinnhubStockPriceQuoteService _stockPriceQuoteService;

        public SelectedStockViewComponent(IFinnhubCompanyProfileService companyProfileService,
                                  IFinnhubStockPriceQuoteService stockPriceQuoteService)
        {
            _companyProfileService = companyProfileService;
            _stockPriceQuoteService = stockPriceQuoteService;
        }
            

        public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
        {
            if (string.IsNullOrWhiteSpace(stockSymbol))
                return Content(string.Empty);

            var profile = await _companyProfileService.GetCompanyProfile(stockSymbol);
            var quote = await _stockPriceQuoteService.GetStockPriceQuote(stockSymbol);

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

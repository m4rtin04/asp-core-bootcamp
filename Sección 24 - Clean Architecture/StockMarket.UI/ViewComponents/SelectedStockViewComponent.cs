using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Core.ServiceContracts;

namespace StockMarket.UI.ViewComponents;

public class SelectedStockViewComponent : ViewComponent
{
    private readonly IFinnhubCompanyProfileService _profileService;
    private readonly IFinnhubStockPriceQuoteService _quoteService;

    public SelectedStockViewComponent(
        IFinnhubCompanyProfileService profileService,
        IFinnhubStockPriceQuoteService quoteService)
    {
        _profileService = profileService;
        _quoteService = quoteService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
    {
        if (string.IsNullOrWhiteSpace(stockSymbol)) return Content(string.Empty);

        var profile = await _profileService.GetCompanyProfile(stockSymbol);
        var quote = await _quoteService.GetStockPriceQuote(stockSymbol);
        if (profile == null || profile.Count == 0) return Content(string.Empty);

        var data = profile.ToDictionary(k => k.Key, v => v.Value?.ToString() ?? string.Empty);
        if (quote != null && quote.TryGetValue("c", out var price))
            data["price"] = Convert.ToDouble(price?.ToString(), CultureInfo.InvariantCulture).ToString("0.00", CultureInfo.InvariantCulture);

        return View(data);
    }
}

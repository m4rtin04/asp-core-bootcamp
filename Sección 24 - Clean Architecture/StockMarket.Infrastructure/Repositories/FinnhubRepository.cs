using System.Text.Json;
using Microsoft.Extensions.Configuration;
using StockMarket.Core.Domain.RepositoryContracts;

namespace StockMarket.Infrastructure.Repositories;

public class FinnhubRepository : IFinnhubRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    // El token se guarda en user-secrets: dotnet user-secrets set "FinnhubToken" "..."
    private string Token => _configuration["FinnhubToken"]
        ?? throw new InvalidOperationException("FinnhubToken is not configured (use user-secrets).");

    private async Task<T?> GetAsync<T>(string url)
    {
        using var client = _httpClientFactory.CreateClient();
        using var response = await client.GetAsync(url);
        var body = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<T>(body);

        if (result is Dictionary<string, object> dict && dict.TryGetValue("error", out var error))
            throw new InvalidOperationException(Convert.ToString(error));

        return result;
    }

    public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol) =>
        GetAsync<Dictionary<string, object>>(
            $"https://finnhub.io/api/v1/stock/profile2?symbol={Uri.EscapeDataString(stockSymbol)}&token={Token}");

    public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol) =>
        GetAsync<Dictionary<string, object>>(
            $"https://finnhub.io/api/v1/quote?symbol={Uri.EscapeDataString(stockSymbol)}&token={Token}");

    public Task<List<Dictionary<string, string>>?> GetStocks() =>
        GetAsync<List<Dictionary<string, string>>>(
            $"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={Token}");

    public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch) =>
        GetAsync<Dictionary<string, object>>(
            $"https://finnhub.io/api/v1/search?q={Uri.EscapeDataString(stockSymbolToSearch)}&token={Token}");
}

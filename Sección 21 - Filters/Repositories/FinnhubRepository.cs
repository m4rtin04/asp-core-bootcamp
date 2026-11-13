using Sección_21___Filters.RepositoryContracts;
using System.Text.Json;

namespace Sección_21___Filters.Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private const string BaseUrl = "https://finnhub.io/api/v1";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private string Token => Uri.EscapeDataString(_configuration["FinnhubToken"] ?? "");

        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
            => GetDictionaryAsync($"{BaseUrl}/stock/profile2?symbol={Uri.EscapeDataString(stockSymbol)}&token={Token}");

        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
            => GetDictionaryAsync($"{BaseUrl}/quote?symbol={Uri.EscapeDataString(stockSymbol)}&token={Token}");

        public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
            => GetDictionaryAsync($"{BaseUrl}/search?q={Uri.EscapeDataString(stockSymbolToSearch)}&token={Token}");

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            string body = await GetJsonAsync($"{BaseUrl}/stock/symbol?exchange=US&token={Token}");
            return JsonSerializer.Deserialize<List<Dictionary<string, string>>>(body);
        }

        private async Task<Dictionary<string, object>?> GetDictionaryAsync(string url)
        {
            string body = await GetJsonAsync(url);
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(body)
                       ?? throw new InvalidOperationException("No response from finnhub server");

            if (dict.TryGetValue("error", out var error) && error != null)
                throw new InvalidOperationException(Convert.ToString(error));

            return dict;
        }

        private async Task<string> GetJsonAsync(string url)
        {
            var client = _httpClientFactory.CreateClient();
            using var response = await client.GetAsync(url);
            string body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Finnhub returned {(int)response.StatusCode}: {body}");

            return body;
        }
    }
}

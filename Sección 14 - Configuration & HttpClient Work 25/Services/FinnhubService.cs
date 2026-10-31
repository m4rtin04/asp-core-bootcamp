using Sección_14___Configuration___HttpClient_Work_23.ServiceContracts;
using System.Text.Json;

namespace Sección_14___Configuration___HttpClient_Work_23.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public  Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        => SendRequestAsync($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}");

        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        => SendRequestAsync($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}");

        private async Task<Dictionary<string, object>?> SendRequestAsync(string url)
        {
            var client = _httpClientFactory.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using var response = await client.SendAsync(request);

            string body = await response.Content.ReadAsStringAsync();

            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            if (dict == null)
                throw new InvalidOperationException("No response from finnhub server");

            if (dict.TryGetValue("error", out var error) && error != null)
                throw new InvalidOperationException(Convert.ToString(error));

            return dict;
        }
    }
}

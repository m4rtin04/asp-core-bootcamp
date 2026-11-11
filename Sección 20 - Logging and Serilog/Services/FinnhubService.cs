using Sección_20___Logging_and_Serilog.RepositoryContracts;
using Sección_20___Logging_and_Serilog.ServiceContracts;
using System.Text.Json;

namespace Sección_20___Logging_and_Serilog.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _repository;

        public FinnhubService(IFinnhubRepository repository) => _repository = repository;

        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
            => _repository.GetCompanyProfile(stockSymbol);

        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
            => _repository.GetStockPriceQuote(stockSymbol);

        public Task<List<Dictionary<string, string>>?> GetStocks()
            => _repository.GetStocks();

        public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
            => _repository.SearchStocks(stockSymbolToSearch);
    }
}

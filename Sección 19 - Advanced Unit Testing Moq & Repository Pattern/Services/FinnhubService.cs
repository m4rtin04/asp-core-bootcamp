using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.RepositoryContracts;
using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.ServiceContracts;
using System.Text.Json;

namespace Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Services
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

using Sección_22___Error_Handling.Exceptions;
using Sección_22___Error_Handling.RepositoryContracts;
using Sección_22___Error_Handling.ServiceContracts;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sección_22___Error_Handling.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IFinnhubRepository _repository;

        public FinnhubService(IFinnhubRepository repository) => _repository = repository;

        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
            => Execute(() => _repository.GetCompanyProfile(stockSymbol));

        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
            => Execute(() => _repository.GetStockPriceQuote(stockSymbol));

        public Task<List<Dictionary<string, string>>?> GetStocks()
            => Execute(() => _repository.GetStocks());

        public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
            => Execute(() => _repository.SearchStocks(stockSymbolToSearch));

        
        private static async Task<T> Execute<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                throw new FinnhubException("Unable to connect to finnhub server", ex);
            }
        }
    }
}

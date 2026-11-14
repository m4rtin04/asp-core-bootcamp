using Sección_23___Solid_Principles.Helpers;
using Sección_23___Solid_Principles.RepositoryContracts;
using Sección_23___Solid_Principles.ServiceContracts;

namespace Sección_23___Solid_Principles.Services
{
    public class FinnhubSearchStocksService : IFinnhubSearchStocksService
    {
        private readonly IFinnhubRepository _repository;

        public FinnhubSearchStocksService(IFinnhubRepository repository) => _repository = repository;

        public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
            => FinnhubCall.Execute(() => _repository.SearchStocks(stockSymbolToSearch));

    }
}

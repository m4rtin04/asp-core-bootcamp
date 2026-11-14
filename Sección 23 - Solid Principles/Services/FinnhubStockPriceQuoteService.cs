using Sección_23___Solid_Principles.Helpers;
using Sección_23___Solid_Principles.RepositoryContracts;
using Sección_23___Solid_Principles.ServiceContracts;

namespace Sección_23___Solid_Principles.Services
{
    public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
    {
        private readonly IFinnhubRepository _repository;

        public FinnhubStockPriceQuoteService(IFinnhubRepository repository) => _repository = repository;

        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
            => FinnhubCall.Execute(() => _repository.GetStockPriceQuote(stockSymbol));
    }
}

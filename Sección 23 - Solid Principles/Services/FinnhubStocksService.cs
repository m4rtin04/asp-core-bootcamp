using Sección_23___Solid_Principles.Helpers;
using Sección_23___Solid_Principles.RepositoryContracts;
using Sección_23___Solid_Principles.ServiceContracts;

namespace Sección_23___Solid_Principles.Services
{
    public class FinnhubStocksService : IFinnhubStocksService
    {
        private readonly IFinnhubRepository _repository;

        public FinnhubStocksService(IFinnhubRepository repository) => _repository = repository;

        public Task<List<Dictionary<string, string>>?> GetStocks()
            => FinnhubCall.Execute(() => _repository.GetStocks());
    }
}

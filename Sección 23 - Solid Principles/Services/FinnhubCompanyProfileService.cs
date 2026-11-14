using Sección_23___Solid_Principles.Helpers;
using Sección_23___Solid_Principles.RepositoryContracts;
using Sección_23___Solid_Principles.ServiceContracts;

namespace Sección_23___Solid_Principles.Services
{
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRepository _repository;

        public FinnhubCompanyProfileService(IFinnhubRepository repository) => _repository = repository;

        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
            => FinnhubCall.Execute(() => _repository.GetCompanyProfile(stockSymbol));
    }
}

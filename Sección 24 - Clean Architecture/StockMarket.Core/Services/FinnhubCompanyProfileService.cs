using Microsoft.Extensions.Logging;
using StockMarket.Core.Domain.RepositoryContracts;
using StockMarket.Core.Exceptions;
using StockMarket.Core.ServiceContracts;

namespace StockMarket.Core.Services;

public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
{
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubCompanyProfileService> _logger;

    public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository, ILogger<FinnhubCompanyProfileService> logger)
    {
        _finnhubRepository = finnhubRepository;
        _logger = logger;
    }

    public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
    {
        try
        {
            return await _finnhubRepository.GetCompanyProfile(stockSymbol);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while connecting to finnhub.io in {Method}", nameof(GetCompanyProfile));
            throw new FinnhubException("Unable to connect to finnhub.io", ex);
        }
    }
}

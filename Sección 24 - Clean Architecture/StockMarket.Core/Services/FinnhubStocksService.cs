using Microsoft.Extensions.Logging;
using StockMarket.Core.Domain.RepositoryContracts;
using StockMarket.Core.Exceptions;
using StockMarket.Core.ServiceContracts;

namespace StockMarket.Core.Services;

public class FinnhubStocksService : IFinnhubStocksService
{
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubStocksService> _logger;

    public FinnhubStocksService(IFinnhubRepository finnhubRepository, ILogger<FinnhubStocksService> logger)
    {
        _finnhubRepository = finnhubRepository;
        _logger = logger;
    }

    public async Task<List<Dictionary<string, string>>?> GetStocks()
    {
        try
        {
            return await _finnhubRepository.GetStocks();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while connecting to finnhub.io in {Method}", nameof(GetStocks));
            throw new FinnhubException("Unable to connect to finnhub.io", ex);
        }
    }
}

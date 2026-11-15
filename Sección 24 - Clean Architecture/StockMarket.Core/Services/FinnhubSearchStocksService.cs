using Microsoft.Extensions.Logging;
using StockMarket.Core.Domain.RepositoryContracts;
using StockMarket.Core.Exceptions;
using StockMarket.Core.ServiceContracts;

namespace StockMarket.Core.Services;

public class FinnhubSearchStocksService : IFinnhubSearchStocksService
{
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubSearchStocksService> _logger;

    public FinnhubSearchStocksService(IFinnhubRepository finnhubRepository, ILogger<FinnhubSearchStocksService> logger)
    {
        _finnhubRepository = finnhubRepository;
        _logger = logger;
    }

    public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
    {
        try
        {
            return await _finnhubRepository.SearchStocks(stockSymbolToSearch);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while connecting to finnhub.io in {Method}", nameof(SearchStocks));
            throw new FinnhubException("Unable to connect to finnhub.io", ex);
        }
    }
}

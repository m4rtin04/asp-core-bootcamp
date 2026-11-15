using Microsoft.Extensions.Logging;
using StockMarket.Core.Domain.RepositoryContracts;
using StockMarket.Core.Exceptions;
using StockMarket.Core.ServiceContracts;

namespace StockMarket.Core.Services;

public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
{
    private readonly IFinnhubRepository _finnhubRepository;
    private readonly ILogger<FinnhubStockPriceQuoteService> _logger;

    public FinnhubStockPriceQuoteService(IFinnhubRepository finnhubRepository, ILogger<FinnhubStockPriceQuoteService> logger)
    {
        _finnhubRepository = finnhubRepository;
        _logger = logger;
    }

    public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
    {
        try
        {
            return await _finnhubRepository.GetStockPriceQuote(stockSymbol);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while connecting to finnhub.io in {Method}", nameof(GetStockPriceQuote));
            throw new FinnhubException("Unable to connect to finnhub.io", ex);
        }
    }
}

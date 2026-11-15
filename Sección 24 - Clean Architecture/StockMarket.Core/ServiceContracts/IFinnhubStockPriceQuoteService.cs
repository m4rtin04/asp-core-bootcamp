namespace StockMarket.Core.ServiceContracts;

public interface IFinnhubStockPriceQuoteService
{
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
}

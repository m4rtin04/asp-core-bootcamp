namespace StockMarket.Core.ServiceContracts;

public interface IFinnhubSearchStocksService
{
    Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
}

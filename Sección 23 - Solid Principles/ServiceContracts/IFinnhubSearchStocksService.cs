namespace Sección_23___Solid_Principles.ServiceContracts
{
    public interface IFinnhubSearchStocksService
    {
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}

namespace Sección_16____CRUDS_Operations.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}

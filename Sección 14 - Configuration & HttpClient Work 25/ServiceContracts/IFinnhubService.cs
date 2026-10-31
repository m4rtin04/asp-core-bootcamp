namespace Sección_14___Configuration___HttpClient_Work_23.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}

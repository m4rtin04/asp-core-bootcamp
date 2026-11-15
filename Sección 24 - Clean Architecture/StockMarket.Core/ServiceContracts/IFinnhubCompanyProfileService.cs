namespace StockMarket.Core.ServiceContracts;

public interface IFinnhubCompanyProfileService
{
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
}

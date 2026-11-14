namespace Sección_23___Solid_Principles.ServiceContracts
{
    public interface IFinnhubStockPriceQuoteService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}

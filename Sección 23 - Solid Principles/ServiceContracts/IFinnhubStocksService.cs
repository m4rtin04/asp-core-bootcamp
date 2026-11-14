namespace Sección_23___Solid_Principles.ServiceContracts
{
    public interface IFinnhubStocksService
    {
        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}

using Sección_20___Logging_and_Serilog.Entities;

namespace Sección_20___Logging_and_Serilog.RepositoryContracts
{
    public interface IStocksRepository
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);
        Task<List<BuyOrder>> GetBuyOrders();
        Task<List<SellOrder>> GetSellOrders();
    }
}

using Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Entities;

namespace Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.RepositoryContracts
{
    public interface IStocksRepository
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);
        Task<List<BuyOrder>> GetBuyOrders();
        Task<List<SellOrder>> GetSellOrders();
    }
}

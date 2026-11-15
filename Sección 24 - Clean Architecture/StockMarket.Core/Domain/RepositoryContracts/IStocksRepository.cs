using StockMarket.Core.Domain.Entities;

namespace StockMarket.Core.Domain.RepositoryContracts;

public interface IStocksRepository
{
    Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
    Task<SellOrder> CreateSellOrder(SellOrder sellOrder);
    Task<List<BuyOrder>> GetBuyOrders();
    Task<List<SellOrder>> GetSellOrders();
}

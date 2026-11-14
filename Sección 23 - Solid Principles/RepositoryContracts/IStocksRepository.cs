using Sección_23___Solid_Principles.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sección_23___Solid_Principles.RepositoryContracts
{
    public interface IStocksRepository
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder);
        Task<SellOrder> CreateSellOrder(SellOrder sellOrder);
        Task<List<BuyOrder>> GetBuyOrders();
        Task<List<SellOrder>> GetSellOrders();
    }
}

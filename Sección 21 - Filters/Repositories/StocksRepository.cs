using Microsoft.EntityFrameworkCore;
using Sección_21___Filters.Entities;
using Sección_21___Filters.RepositoryContracts;

namespace Sección_21___Filters.Repositories
{
    public class StocksRepository : IStocksRepository
    {
        private readonly StockMarketDbContext _db;

        public StocksRepository(StockMarketDbContext db) => _db = db;

        public async Task<BuyOrder> CreateBuyOrder(BuyOrder buyOrder)
        {
            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder sellOrder)
        {
            _db.SellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<List<BuyOrder>> GetBuyOrders()
            => await _db.BuyOrders.AsNoTracking().ToListAsync();

        public async Task<List<SellOrder>> GetSellOrders()
            => await _db.SellOrders.AsNoTracking().ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Entities
{
    public class StockMarketDbContext : DbContext
    {
        public StockMarketDbContext(DbContextOptions<StockMarketDbContext> options) : base(options)
        {
        }

        public DbSet<BuyOrder> BuyOrders => Set<BuyOrder>();
        public DbSet<SellOrder> SellOrders => Set<SellOrder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");
        }
    }
}

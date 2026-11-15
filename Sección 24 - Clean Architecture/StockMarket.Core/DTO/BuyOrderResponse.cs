using StockMarket.Core.Domain.Entities;

namespace StockMarket.Core.DTO;

public class BuyOrderResponse
{
    public Guid BuyOrderID { get; set; }
    public string? StockSymbol { get; set; }
    public string? StockName { get; set; }
    public DateTime DateAndTimeOfOrder { get; set; }
    public uint Quantity { get; set; }
    public double Price { get; set; }
    public double TradeAmount { get; set; }
}

public static class BuyOrderExtensions
{
    public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder order) => new()
    {
        BuyOrderID = order.BuyOrderID,
        StockSymbol = order.StockSymbol,
        StockName = order.StockName,
        DateAndTimeOfOrder = order.DateAndTimeOfOrder,
        Quantity = order.Quantity,
        Price = order.Price,
        TradeAmount = order.Quantity * order.Price
    };
}

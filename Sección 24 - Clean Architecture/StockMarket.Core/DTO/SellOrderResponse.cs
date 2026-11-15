using StockMarket.Core.Domain.Entities;

namespace StockMarket.Core.DTO;

public class SellOrderResponse
{
    public Guid SellOrderID { get; set; }
    public string? StockSymbol { get; set; }
    public string? StockName { get; set; }
    public DateTime DateAndTimeOfOrder { get; set; }
    public uint Quantity { get; set; }
    public double Price { get; set; }
    public double TradeAmount { get; set; }
}

public static class SellOrderExtensions
{
    public static SellOrderResponse ToSellOrderResponse(this SellOrder order) => new()
    {
        SellOrderID = order.SellOrderID,
        StockSymbol = order.StockSymbol,
        StockName = order.StockName,
        DateAndTimeOfOrder = order.DateAndTimeOfOrder,
        Quantity = order.Quantity,
        Price = order.Price,
        TradeAmount = order.Quantity * order.Price
    };
}

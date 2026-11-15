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
    public static SellOrderResponse ToSellOrderResponse(this Domain.Entities.SellOrder sellOrder)
    {
        return new SellOrderResponse
        {
            SellOrderID = sellOrder.SellOrderID,
            StockSymbol = sellOrder.StockSymbol,
            StockName = sellOrder.StockName,
            DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
            Quantity = sellOrder.Quantity,
            Price = sellOrder.Price,
            TradeAmount = sellOrder.Quantity * sellOrder.Price
        };
    }
}

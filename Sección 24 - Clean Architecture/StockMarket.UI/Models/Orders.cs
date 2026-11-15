using StockMarket.Core.DTO;

namespace StockMarket.UI.Models;

public class Orders
{
    public List<BuyOrderResponse> BuyOrders { get; set; } = new();
    public List<SellOrderResponse> SellOrders { get; set; } = new();
}

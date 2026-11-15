using System.ComponentModel.DataAnnotations;
using StockMarket.Core.Domain.Entities;

namespace StockMarket.Core.DTO;

public class SellOrderRequest : OrderRequest
{
    public SellOrder ToSellOrder()
    {
        return new SellOrder
        {
            StockSymbol = this.StockSymbol,
            StockName = this.StockName,
            DateAndTimeOfOrder = this.DateAndTimeOfOrder,
            Quantity = this.Quantity,
            Price = this.Price
        };
    }
}
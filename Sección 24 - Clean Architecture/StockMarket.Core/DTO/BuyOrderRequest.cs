using StockMarket.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace StockMarket.Core.DTO;

public class BuyOrderRequest : OrderRequest
{
    public BuyOrder ToBuyOrder()
    {
        return new BuyOrder
        {
            StockSymbol = StockSymbol,
            StockName = StockName,
            DateAndTimeOfOrder = DateAndTimeOfOrder,
            Quantity = Quantity,
            Price = Price
        };

    }
}

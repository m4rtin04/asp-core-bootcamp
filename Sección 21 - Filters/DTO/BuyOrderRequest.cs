using Sección_21___Filters.Entities;
using Sección_21___Filters.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Sección_21___Filters.DTO
{
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
}

using Sección_22___Error_Handling.Entities;
using Sección_22___Error_Handling.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Sección_22___Error_Handling.DTO
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

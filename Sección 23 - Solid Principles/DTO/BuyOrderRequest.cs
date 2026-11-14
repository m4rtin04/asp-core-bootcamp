using Sección_23___Solid_Principles.Entities;
using Sección_23___Solid_Principles.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Sección_23___Solid_Principles.DTO
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

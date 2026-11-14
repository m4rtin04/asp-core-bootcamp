using Sección_23___Solid_Principles.Entities;
using Sección_23___Solid_Principles.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Sección_23___Solid_Principles.DTO
{
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
}

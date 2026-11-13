using Sección_21___Filters.Entities;
using Sección_21___Filters.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Sección_21___Filters.DTO
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

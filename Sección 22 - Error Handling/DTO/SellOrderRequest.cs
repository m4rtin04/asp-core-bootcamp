using Sección_22___Error_Handling.Entities;
using Sección_22___Error_Handling.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Sección_22___Error_Handling.DTO
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

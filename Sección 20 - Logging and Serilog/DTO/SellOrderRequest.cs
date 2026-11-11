using Sección_20___Logging_and_Serilog.Entities;
using Sección_20___Logging_and_Serilog.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Sección_20___Logging_and_Serilog.DTO
{
    public class SellOrderRequest
    {
        [Required(ErrorMessage = "Stock symbol can't be null or empty")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock name can't be null or empty")]
        public string? StockName { get; set; }

        [MinimumDate("2000-01-01", ErrorMessage = "Date of the order should not be older than Jan 01, 2000")]
        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity should be between 1 and 100000")]
        public uint Quantity { get; set; }

        [Range(1.0, 10000.0, ErrorMessage = "Price should be between 1 and 10000")]
        public double Price { get; set; }

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

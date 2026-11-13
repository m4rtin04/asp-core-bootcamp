using Sección_21___Filters.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
namespace Sección_21___Filters.DTO
{
    public abstract class OrderRequest
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
    }

}
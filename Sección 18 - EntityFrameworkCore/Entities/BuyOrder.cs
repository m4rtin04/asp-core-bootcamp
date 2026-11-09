using System.ComponentModel.DataAnnotations;

namespace Sección_18___EntityFrameworkCore.Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }

        [Required(ErrorMessage = "Stock Symbol is required")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name is required")]
        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
        public uint Quantity { get; set; }

        [Range(1.0, 10000.0, ErrorMessage = "Price must be between 1 and 10000")]
        public double Price
        {
            get; set;
        }
    }
}

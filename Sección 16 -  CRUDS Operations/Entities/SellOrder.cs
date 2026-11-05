using System.ComponentModel.DataAnnotations;

namespace Sección_16____CRUDS_Operations.Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }

        [Required(ErrorMessage = "Stock symbol can't be null or empty")]
        public string? StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock name can't be null or empty")]
        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "Quantity should be between 1 and 100000")]
        public uint Quantity { get; set; }

        [Range(1.0, 10000.0, ErrorMessage = "Price should be between 1 and 10000")]
        public double Price { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace StockMarket.Core.Domain.Entities;

/// <summary>Entidad que representa una orden de compra.</summary>
public class BuyOrder
{
    [Key]
    public Guid BuyOrderID { get; set; }

    [Required(ErrorMessage = "Stock symbol can't be null or empty")]
    public string StockSymbol { get; set; } = null!;

    [Required(ErrorMessage = "Stock name can't be null or empty")]
    public string StockName { get; set; } = null!;

    public DateTime DateAndTimeOfOrder { get; set; }

    [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
    public uint Quantity { get; set; }

    [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
    public double Price { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace StockMarket.Core.Domain.Entities;

/// <summary>Entidad que representa una orden de venta.</summary>
public class SellOrder
{
    [Key]
    public Guid SellOrderID { get; set; }

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

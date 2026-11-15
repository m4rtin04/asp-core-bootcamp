using System.ComponentModel.DataAnnotations;
using StockMarket.Core.Domain.Entities;

namespace StockMarket.Core.DTO;

public class SellOrderRequest : IValidatableObject
{
    [Required(ErrorMessage = "Stock symbol can't be null or empty")]
    public string? StockSymbol { get; set; }

    [Required(ErrorMessage = "Stock name can't be null or empty")]
    public string? StockName { get; set; }

    public DateTime DateAndTimeOfOrder { get; set; }

    [Range(1, 100000, ErrorMessage = "Quantity must be between 1 and 100000")]
    public uint Quantity { get; set; }

    [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
    public double Price { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateAndTimeOfOrder < new DateTime(2000, 1, 1))
        {
            yield return new ValidationResult(
                "Date of the order should not be older than Jan 01, 2000.",
                new[] { nameof(DateAndTimeOfOrder) });
        }
    }

    public SellOrder ToSellOrder() => new()
    {
        SellOrderID = Guid.NewGuid(),
        StockSymbol = StockSymbol!,
        StockName = StockName!,
        DateAndTimeOfOrder = DateAndTimeOfOrder,
        Quantity = Quantity,
        Price = Price
    };
}

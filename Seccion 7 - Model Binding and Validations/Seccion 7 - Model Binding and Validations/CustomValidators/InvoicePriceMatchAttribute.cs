using System.ComponentModel.DataAnnotations;

namespace Seccion_7___Model_Binding_and_Validations.CustomValidators
{
    public class InvoicePriceMatchAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var order = (Models.Order)validationContext.ObjectInstance;
            
            if (order.InvoicePrice.HasValue && order.Products != null && order.Products.Any())
            {
                double calculatedTotal = order.Products.Sum(p => (p.Price ?? 0) * (p.Quantity ?? 0));

                if (calculatedTotal != order.InvoicePrice.Value)
                {
                    return new ValidationResult("InvoicePrice doesn't match with the total cost of the specified products in the order.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

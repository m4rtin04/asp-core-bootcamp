using Seccion_7___Model_Binding_and_Validations.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace Seccion_7___Model_Binding_and_Validations.Models
{
    public class Order
    {
        [Required(ErrorMessage = "Order number is required.")]
        public int? OrderNo { get; set; }

        [Required(ErrorMessage = "Order date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Order date must be a valid date.")]
        public DateTime? OrderDate { get; set; }

        [Required(ErrorMessage = "Invoice price is required.")]
        [InvoicePriceMatch]
        public Double? InvoicePrice { get; set; }

        [Required(ErrorMessage = "Products are required.")]
        [MinLength(1, ErrorMessage = "At least one product is required.")]
        public List<Product> Products { get; set; } = new List<Product>();
    }
}

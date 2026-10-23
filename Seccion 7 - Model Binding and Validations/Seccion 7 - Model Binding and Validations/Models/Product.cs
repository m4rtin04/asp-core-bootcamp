using System.ComponentModel.DataAnnotations;

namespace Seccion_7___Model_Binding_and_Validations.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Product code is required.")]
        public int? ProductCode { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public Double? Price { get; set; }
        [Required(ErrorMessage = "Quantity is required.")]
        public int? Quantity { get; set; }
    }
}

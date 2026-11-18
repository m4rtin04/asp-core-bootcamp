using System.ComponentModel.DataAnnotations;

namespace Sección_25___Identity__Authorization_and_Security.DTO
{
    public class ProfileDTO
    {
        [Required(ErrorMessage = "Display name is required")]
        [StringLength(50)]
        public string? DisplayName { get; set; }
    }
}

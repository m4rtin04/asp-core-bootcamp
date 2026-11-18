using System.ComponentModel.DataAnnotations;

namespace Sección_25___Identity__Authorization_and_Security.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

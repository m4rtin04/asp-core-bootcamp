using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Sección_25___Identity__Authorization_and_Security.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Display name is required")]
        [StringLength(50)]
        public string? DisplayName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [Remote(action: "IsEmailAvailable", controller: "Account", ErrorMessage = "This email is already registered")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string? ConfirmPassword { get; set; }
    }
}

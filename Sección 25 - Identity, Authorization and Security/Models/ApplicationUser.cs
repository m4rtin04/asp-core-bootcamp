using Microsoft.AspNetCore.Identity;

namespace Sección_25___Identity__Authorization_and_Security.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

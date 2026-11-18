namespace Sección_25___Identity__Authorization_and_Security.DTO
{
    public class UserRowDTO
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }
        public bool IsAdmin { get; set; }
    }
}

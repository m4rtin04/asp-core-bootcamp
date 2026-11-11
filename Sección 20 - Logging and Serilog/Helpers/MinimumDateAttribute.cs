using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Sección_20___Logging_and_Serilog.Helpers
{
    public class MinimumDateAttribute : ValidationAttribute
    {
        public DateTime MinimumDate { get; }

        public MinimumDateAttribute(string minimumDate)
        {
            MinimumDate = DateTime.Parse(minimumDate, CultureInfo.InvariantCulture);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date && date < MinimumDate)
                return new ValidationResult(
                    ErrorMessage ?? $"Date should not be older than {MinimumDate:yyyy-MM-dd}");

            return ValidationResult.Success;
        }
    }
}

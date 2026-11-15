using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Core.Helpers
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

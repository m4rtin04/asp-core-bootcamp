using System.ComponentModel.DataAnnotations;

namespace Sección_20___Logging_and_Serilog.Helpers
{
    internal static class ValidationHelper
    {
        internal static void ModelValidation(object obj)
        {
            ValidationContext context = new ValidationContext(obj);
            List<ValidationResult> results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, context, results, true);

            if (!isValid)
            {
                throw new ArgumentException(results.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}

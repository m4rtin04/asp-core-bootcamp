using System.ComponentModel.DataAnnotations;

namespace Sección_17___Tag_Helpers.Helpers
{
    internal static class ValidationHelpers
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
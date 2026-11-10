using System.ComponentModel.DataAnnotations;

namespace Sección_19___Advanced_Unit_Testing_Moq___Repository_Pattern.Helpers
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

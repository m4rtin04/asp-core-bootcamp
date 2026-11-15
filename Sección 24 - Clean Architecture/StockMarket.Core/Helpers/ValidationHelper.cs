using System.ComponentModel.DataAnnotations;

namespace StockMarket.Core.Helpers;

public static class ValidationHelper
{
    /// <summary>Valida el objeto (incluye IValidatableObject) y lanza ArgumentException si hay errores.</summary>
    public static void ModelValidation(object obj)
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

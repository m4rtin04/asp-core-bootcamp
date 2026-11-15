using System.ComponentModel.DataAnnotations;

namespace StockMarket.Core.Helpers;

public static class ValidationHelper
{
    /// <summary>Valida el objeto (incluye IValidatableObject) y lanza ArgumentException si hay errores.</summary>
    public static void ModelValidation(object obj)
    {
        var context = new ValidationContext(obj);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(obj, context, results, validateAllProperties: true))
            throw new ArgumentException(results.First().ErrorMessage);
    }
}

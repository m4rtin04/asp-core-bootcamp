using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sección_23___Solid_Principles.Helpers
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

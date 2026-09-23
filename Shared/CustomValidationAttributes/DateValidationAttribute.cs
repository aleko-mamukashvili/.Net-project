using System.ComponentModel.DataAnnotations;

namespace Shared.CustomValidationAttributes;

public class DateValidationAttribute(int minimumAge) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is DateTime dateValue)
        {
            var allowedDate = new DateTime(DateTime.Now.Year - minimumAge, DateTime.Now.Month, DateTime.Now.Day);

            if (dateValue.Date <= allowedDate.Date)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"The field {validationContext.DisplayName} must contain a date before {allowedDate}.");

        }

        return new ValidationResult("Invalid data type.");
    }
}

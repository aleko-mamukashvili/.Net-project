using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Shared.CustomValidationAttributes;

public class SingleLanguageRestrictionAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is string stringValue)
        {
            var georgianPattern = "^[\\u10A0-\\u10FF\\s]*$";
            var englishPattern = "^[a-zA-Z\\s]*$";

            bool isGeorgian = Regex.IsMatch(stringValue, georgianPattern);
            bool isEnglish = Regex.IsMatch(stringValue, englishPattern);

            if (isGeorgian ^ isEnglish)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"The field {validationContext.DisplayName} must contain either only Georgian or only English characters, but not both.");
        }

        return new ValidationResult("Invalid data type.");
    }
}

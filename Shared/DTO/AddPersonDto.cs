using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shared.CustomValidationAttributes;

namespace Shared.DTO;

public class AddPersonDto
{
    [MinLength(2)]
    [MaxLength(50)]
    [SingleLanguageRestriction]
    public required string Name { get; set; }
    [MinLength(2)]
    [MaxLength(50)]
    [SingleLanguageRestriction]
    public required string Surname { get; set; }
    [AllowedValues("ქალი", "კაცი")]
    public required string Gender { get; set; }
    [StringLengthFixedValidation(11)]
    public required string PersonalNumber { get; set; }
    [DateValidation(18)]
    public required DateTime BirthDate { get; set; }
    public required CityDto City { get; set; }
    public required IFormFile Image { get; set; }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shared.CustomValidationAttributes;

namespace Shared.DTO;

public class UpdatePersonDto
{
    [MinLength(2)]
    [MaxLength(50)]
    [SingleLanguageRestriction]
    public string? Name { get; set; }
    [MinLength(2)]
    [MaxLength(50)]
    [SingleLanguageRestriction]
    public string? Surname { get; set; }
    [AllowedValues("ქალი", "კაცი", null)]
    public string? Gender { get; set; }
    [StringLengthFixedValidation(11)]
    public string? PersonalNumber { get; set; }
    [DateValidation(18)]
    public DateTime? BirthDate { get; set; }
    public CityDto? City { get; set; }
    public IFormFile? Image { get; set; }
}

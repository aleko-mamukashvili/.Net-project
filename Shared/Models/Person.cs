using System.ComponentModel.DataAnnotations;
using Shared.CustomValidationAttributes;

namespace Shared.Models;

public class Person
{
    [Required]
    public int PersonId { get; set; }
    [MinLength(2)]
    [MaxLength(50)]
    [SingleLanguageRestriction]
    public required string Name { get; set; }
    [MinLength(2)]
    [MaxLength(50)]
    [SingleLanguageRestriction]
    public required string Surname { get; set; }
    [AllowedValues("ქალი", "კაცი")]
    [MaxLength(50)]
    public required string Gender { get; set; }
    [MaxLength(11)]
    [StringLengthFixedValidation(11)]
    public required string PersonalNumber { get; set; }
    [Required]
    [DateValidation(18)]
    public DateTime BirthDate { get; set; }
    public required City City { get; set; }
    public required List<PhoneNumber> PhoneNumber { get; set; }
    [MaxLength(50)]
    public required string Image { get; set; }
    public required List<RelatedPerson> RelatedPerson { get; set; }
}

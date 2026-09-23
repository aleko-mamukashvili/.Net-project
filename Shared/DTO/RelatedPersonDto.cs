using System.ComponentModel.DataAnnotations;

namespace Shared.DTO;

public class RelatedPersonDto
{
    public int PersonId { get; set; }
    public int PersonRelatedId { get; set; }

    [AllowedValues("კოლეგა", "ნაცნობი", "ნათესავი", "სხვა")]
    [MaxLength(10)]
    public required string PersonType { get; set; }
}

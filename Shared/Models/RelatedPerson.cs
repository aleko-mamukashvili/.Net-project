using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models;

public class RelatedPerson
{ 
    public int RelatedPersonId { get; set; }

    [JsonIgnore]
    public Person? RelatedPersonPerson { get; set; }
    public int RelatedPersonPersonId { get; set; }
    [JsonIgnore]
    public Person? PersonRelated { get; set; }
    public int PersonRelatedId { get; set; }

    [AllowedValues("კოლეგა", "ნაცნობი", "ნათესავი", "სხვა")]
    [MaxLength(10)]
    public required string PersonType { get; set; }
}

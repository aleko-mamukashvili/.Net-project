using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models;

public class PhoneNumber
{
    public int PhoneId { get; set; }
    [MinLength(4)]
    [MaxLength(50)]
    public required string Number { get; set; }
    [AllowedValues("მობილური", "ოფისის", "სახლის")]
    [MaxLength (10)]
    public required string NumberType { get; set; }
    public int PhonePersonId { get; set; }
    [JsonIgnore]
    public Person? Person { get; set; }
}
